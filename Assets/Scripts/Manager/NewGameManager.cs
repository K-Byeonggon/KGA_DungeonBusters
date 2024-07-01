using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewGameManager : NetworkBehaviour
{
    public static NewGameManager Instance;

    //게임 상태 변수
    [SerializeField] GameState _currentState;
    [SerializeField] int _currentDungeon;                               //현재 진행중인 던전
    [SerializeField] int _currentStage;                                 //현재 진행중인 스테이지
    [SerializeField] Monster _currentMonster;                           //현재 전투중인 몬스터
    [SerializeField] Queue<Monster> _currentDungeonMonsterQueue;        //현재 진행중인 던전에 있는 몬스터를 담은 Queue
    [SerializeField] Dictionary<int, int> _submittedCardList;
    [SerializeField] Dictionary<int, int> _duplicationCheck;
    [SerializeField] List<int> _bonusJewels;

    [SerializeField] int _currentMonsterId;

    public Dictionary<int, Monster> _monsterList = new Dictionary<int, Monster>();

    #region 프로퍼티
    public int CurrentDungeon
    {
        get { return _currentDungeon; }
        set
        {
            _currentDungeon = value;
            //여기서 UI 변경? 하면 클라도 값이 변경될테니 UI 변경 되겠지?
            Debug.Log("Update CurrentDungeon");
            BattleUIManager.Instance.RequestUpdateDungeon();
        }
    }

    public int CurrentStage
    {
        get { return _currentStage; }
        set
        {
            _currentStage = value;
            Debug.Log("Update CurrentStage");
            BattleUIManager.Instance.RequestUpdateStage();
        }
    }

    public Monster CurrentMonster
    {
        get { return _currentMonster; }
        set
        {
            _currentMonster = value;
            Debug.Log("Update CurrentMonster");
            BattleUIManager.Instance.RequestUpdateMonster();
        }
    }

    public int CurrentMonsterId
    {
        get { return _currentMonsterId; }
        set { _currentMonsterId = value; }
    }

    public Queue<Monster> CurrentDungeonMonsterQueue
    {
        get { return _currentDungeonMonsterQueue; }
        set { _currentDungeonMonsterQueue = value; }
    }

    public Dictionary<int, int> SubmittedCardList
    {
        get { return _submittedCardList; }
        set { _submittedCardList = value; }
    }

    public Dictionary<int, int> DuplicationCheck
    {
        get { return _duplicationCheck; }
        set { _duplicationCheck = value; }
    }

    public List<int> BonusJewels
    {
        get { return _bonusJewels; }
        set
        {
            _bonusJewels = value;
            BattleUIManager.Instance.RequestUpdateBonusJewels();
        }
    }

    #endregion

    private void Awake()
    {
        Instance = this;
    }


    public override void OnStartServer()
    {
        base.OnStartServer();
        InitializeGame();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        //RequestCurrentDungeonInfo();
        CmdSendAllStateToClient();
    }



    #region 상태머신
    [Server]
    private void ChangeState(GameState newState)
    {
        _currentState = newState;
        OnStateEnter(newState);
    }

    [Server]
    private void OnStateEnter(GameState state)
    {
        switch (state)
        {
            case GameState.StartDungeon:
                StartDungeon();
                break;
            case GameState.StartStage:
                StartStage();
                break;
            case GameState.SubmitCard:
                break;
            case GameState.WaitForPlayers:
                break;
            case GameState.CalculateResults:
                break;
            case GameState.GetJewels:
                break;
            case GameState.LoseJewels:
                break;
            case GameState.EndGame:
                break;
        }
    }

    #endregion




    #region 1. 게임 초기화

    //게임 초기화 함수(서버에서만 실행)
    [Server]
    private void InitializeGame()
    {
        CurrentDungeon = 0;
        CurrentStage = 0;
        SubmittedCardList = new Dictionary<int, int>();
        DuplicationCheck = new Dictionary<int, int>();
        BonusJewels = new List<int>() { 0, 0, 0 };
        ChangeState(GameState.StartDungeon);
    }

    #endregion


    #region 2. 던전 시작

    // 던전 시작 함수(서버에서만 실행 후, ClientRpc로 각 클라에 상태 업데이트)
    
    [Server]
    private void StartDungeon()
    {
        CurrentDungeon++;
        this.Enqueue4MonstersFromData(_currentDungeon);

        //RpcUpdateDungeonState(CurrentDungeon);

        ChangeState(GameState.StartStage);
    }

    [Command(requiresAuthority = false)]
    private void RequestCurrentDungeonInfo()
    {
        RpcUpdateDungeonState(CurrentDungeon);
    }

    // 클라이언트에서 던전 상태 업데이트
    [ClientRpc]
    private void RpcUpdateDungeonState(int dungeon)
    {
        Debug.Log("ClientRpc: UpdateDungeonState");
        CurrentDungeon = dungeon;
    }

    #endregion

    #region 2-1. 스테이지 시작

    //스테이지 시작전 초기화: 플레이어들이 제출한 카드, 플레이어 보석개수, 사용한 카드

    [Server]
    private void StartStage()
    {
        CurrentStage++;
        this.DequeueMonsterCurrentStage();

        //RpcUpdateStageState(CurrentStage, CurrentMonster);

        ChangeState(GameState.SubmitCard);
    }

    [ClientRpc]
    private void RpcUpdateStageState(int stage, int monsterId)
    {
        Debug.Log("ClientRpc: UpdateStageState");
        CurrentStage = stage;

        CurrentMonster = DataManager.Instance.GetMonster(monsterId);

        SetLocalPopupSelect();
    }
    //StartServer에서 Server를 다 바꾸고, StartClient에서 클라를 갱신
    [Command(requiresAuthority = false)]
    private void CmdSendAllStateToClient()
    {
        RpcUpdateDungeonState(CurrentDungeon);
        //여기서 몬스터ID를 주면, 클라에선 다시 xml에서 Id로 몬스터 정보 받기.
        RpcUpdateStageState(CurrentStage, CurrentMonster.DataId);
    }

    #endregion

    #region 2-2. 플레이어 카드 세팅

    private void SetLocalPopupSelect()
    {
        BattleUIManager.Instance.RequestUpdateSelectCard();
    }

    #endregion



    #region 3. 카드 제출 처리

    // 이건 모든 클라에서 카드 제출 하는건가?
    public void OnSubmitCard(int card)
    {
        if (isClient)
        {
            //CmdSubmitCard(NetworkClient.connection.identity.GetComponent<MyPlayer>().playerId, card);
        }
    }

    //플레이어의 카드 제출 처리(클라에서 요청후, 서버에서 처리)
    [Command(requiresAuthority = false)]
    public void CmdAddSubmittedCard(NetworkIdentity identity, int cardNum)
    {
        int playerNetId = (int)identity.netId;

        if (SubmittedCardList.ContainsKey(playerNetId))
        {
            SubmittedCardList[playerNetId] = cardNum;
        }
        else
        {
            SubmittedCardList.Add(playerNetId, cardNum);
        }
        Debug.Log($"Added- netId:{playerNetId}, num:{SubmittedCardList[playerNetId]}");

        if (AllPlayersSubmitted())
        {
            OnAllPlayersSubmitted();
        }
    }

    private bool AllPlayersSubmitted()
    {
        return NetworkServer.connections.Count == SubmittedCardList.Count;
    }

    private void OnAllPlayersSubmitted()
    {
        CalculateDamage();
    }

    [Server]
    private void CalculateDamage()
    {
        int monsterHp = CurrentMonster.HP;
        int totalDamage = 0;
        
        foreach (var card in SubmittedCardList.Values)
        {
            if (DuplicationCheck.ContainsKey(card))
            {
                DuplicationCheck[card]++;
            }
            else
            {
                DuplicationCheck[card] = 1;
            }
        }

        foreach(var kvp in SubmittedCardList)
        {
            if (DuplicationCheck[kvp.Value] == 1)
            {
                totalDamage += kvp.Value;
            }
        }

        bool StageClear = totalDamage >= monsterHp;

        Debug.Log($"totalDamage == {totalDamage}");
        if(StageClear) { CmdChooseRewardedPlayer(); }
        else { /*보석 잃는 로직*/ }
    }



    #region 보석 보상 로직
    private void RemoveDuplicatedCard()
    {
        foreach (int card in SubmittedCardList.Values)
        {
            if (DuplicationCheck[card] > 1)
            {
                int valueToRemove = card;
                var keysToRemove = SubmittedCardList.Where(kvp => kvp.Value == valueToRemove)
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (var key in keysToRemove)
                {
                    SubmittedCardList.Remove(key);
                    Debug.Log($"Key {key} with Value {valueToRemove} removed");
                }
            }
        }
    }


    //토벌 성공시는 보상을 받을 가장 작은 값이 여러개일 경우가 없지만,
    //토벌 실패시는 보상을 잃을 가장 작은 값이 여러개일 수 있다.
    private List<int> GetMinCardPlayerNetIds()
    {
        int minValue = SubmittedCardList.Values.Min();
        List<int> minCardPlayerNetIds = SubmittedCardList
            .Where(kv => kv.Value == minValue)
            .Select(kv => kv.Key)
            .ToList();
        return minCardPlayerNetIds;
    }

    private MyPlayer GetPlayerFromNetId(int playerNetId)
    {   
        MyPlayer player = null; NetworkIdentity networkIdentity;
        if (NetworkServer.spawned.TryGetValue((uint)playerNetId, out networkIdentity))
        {
            player = networkIdentity.gameObject.GetComponent<MyPlayer>();
            return player;
        }
        else { return player; }
    }

    [Command(requiresAuthority = false)]
    public void CmdChooseRewardedPlayer()
    {
        //0. 중복 카드 제거
        RemoveDuplicatedCard();
        
        for (int reward_n = 0; reward_n < 3; reward_n++)
        {
            if (CurrentMonster.Reward[reward_n] == null)
                break;
            //1. 가장 작은 카드 낸 플레이어 NetId 구하기(그 후 Dic에서 삭제) (1명으로 test하면 보상이 2개 이상일때 에러 발생)
            List<int> playerNetIds = GetMinCardPlayerNetIds();
            int usedCard = SubmittedCardList[playerNetIds[0]];
            SubmittedCardList.Remove(playerNetIds[0]);


            //2. 모든 클라의 해당 NetId가진 플레이어에 보상 주기.
            foreach (int playerNetId in playerNetIds)
            {
                RpcPlayerGetReward(playerNetId, reward_n);
                RpcSetPlayerUsedCard(playerNetId, usedCard);
            }

            //3. 모든 클라의 해당 NetId가진 플레이어 가진 카드, 사용한 카드 갱신 (가진 카드는 모두 바꿀 필요없지만)
            
        }
    }

    [ClientRpc]
    public void RpcPlayerGetReward(int playerNetId, int reward_n)
    {
        //2-1. NetId의 플레이어 찾기
        MyPlayer player = GetPlayerFromNetId(playerNetId);

        //2-2. 플레이어에 해당 Reward의 보석 추가.
        if (CurrentMonster.Reward[reward_n] != null)
        {
            player.Jewels[0] += CurrentMonster.Reward[reward_n][0];
            player.Jewels[1] += CurrentMonster.Reward[reward_n][1];
            player.Jewels[2] += CurrentMonster.Reward[reward_n][2];
        }
    }

    [ClientRpc]
    public void RpcSetPlayerUsedCard(int playerNetId, int usedCard)
    {
        MyPlayer player = GetPlayerFromNetId(playerNetId);
        player.UsedCards.Add(usedCard);
        player.Cards.Remove(usedCard);
    }

    #endregion

    #region 보석 잃는 로직



    #endregion


    #endregion


    #region 4. 전투 결과 계산 및 보상 분배
    //전투 결과 계산 및 보상 분배 (서버에서 실행 후, ClientRpc로 각 클라에 상태 업데이트)
    [Server]
    private void CalculateBattleResult()
    {
        //전투 결과 계산 메서드
        //보상 분배 메서드
        RpcUpdateGameState();
    }

    //상태 동기화 함수
    [ClientRpc]
    private void RpcUpdateGameState()
    {
        //클라이언트에 게임 상태 업데이트
    }
    #endregion


}
