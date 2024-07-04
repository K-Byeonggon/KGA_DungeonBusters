using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewGameManager : NetworkBehaviour
{
    public static NewGameManager Instance;

    //게임 상태 변수
    [SerializeField] GameState _currentState;
    [SerializeField][SyncVar(hook = nameof(OnChangeCurrentDungeon))]
    int _currentDungeon;                               //현재 진행중인 던전
    
    [SerializeField][SyncVar(hook = nameof(OnChangeCurrentStage))]
    int _currentStage;                                 //현재 진행중인 스테이지

    [SerializeField]
    Monster _currentMonster;                           //현재 전투중인 몬스터
    [SerializeField][SyncVar(hook = nameof(OnChangeCurrentMonsterId))]
    int _currentMonsterId;                              //클라에선 MonsterId를 받아 currentMonster를 변경한다.

    [SerializeField]
    [SyncVar(hook = nameof(OnChangeBonusJewels))]
    List<int> _bonusJewels;                            //보너스 Jewel

    [SerializeField] Queue<Monster> _currentDungeonMonsterQueue;        //현재 진행중인 던전에 있는 몬스터를 담은 Queue
    [SerializeField] Dictionary<int, int> _submittedCardList;           //key:netId, value:제출한 카드Num
    [SerializeField] Dictionary<int, int> _duplicationCheck;            //key:CardNum, value:해당Num의 개수
    [SerializeField] Dictionary<uint, int> _selectedJewelIndexList;     //key:netId, value:플레이어가 선택한 버릴 Jewel 인덱스
    [SerializeField] Dictionary<int, List<int>> _netIdAndJewelsIndex;    //key:netId, value:가장많은Jewel의 인덱스List

    public Dictionary<int, Monster> _monsterList = new Dictionary<int, Monster>();

    #region 프로퍼티

    public int CurrentDungeon
    {
        get { return _currentDungeon; }
        set { _currentDungeon = value; }
    }

    public int CurrentStage
    {
        get { return _currentStage; }
        set { _currentStage = value; }
    }

    public Monster CurrentMonster
    {
        get { return _currentMonster; }
        set
        {
            _currentMonster = value;
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
        set { _bonusJewels = value; }
    }

    public Dictionary<uint, int> SelectedJewelIndexList
    {
        get { return _selectedJewelIndexList; }
        set { _selectedJewelIndexList = value; }
    }

    public Dictionary<int, List<int>> NetIdAndJewelsIndex
    {
        get { return _netIdAndJewelsIndex; }
        set { _netIdAndJewelsIndex = value; }
    }

    #endregion

    #region hook함수

    private void OnChangeCurrentDungeon(int oldDungeon, int newDungeon)
    {
        BattleUIManager.Instance.RequestUpdateDungeon();
    }

    private void OnChangeCurrentStage(int oldStage, int newStage)
    {
        BattleUIManager.Instance.RequestUpdateStage();
    }

    private void OnChangeCurrentMonsterId(int oldMonsterId, int newMonsterId)
    {
        CurrentMonster = DataManager.Instance.GetMonster(newMonsterId);
    }

    private void OnChangeBonusJewels(List<int> oldJewels, List<int> newJewels)
    {
        BattleUIManager.Instance.RequestUpdateBonusJewels();
    }

    #endregion


    private void Awake()
    {
        Instance = this;
    }


    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if(isServer) InitializeGame();
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
                SetLocalPopupSelect();
                break;
            case GameState.CalculateResults:
                ServerDecideStageResult();
                break;
            case GameState.GetJewels:
                ServerChooseRewardedPlayer();
                break;
            case GameState.LoseJewels:
                ServerChooseLoseJewelsPlayer();
                break;
            case GameState.EndStage:
                ServerCheckStageOver();
                break;
            case GameState.EndDungeon:
                ServerCheckDungeonOver();
                break;
            case GameState.EndGame:
                TempEndGame();
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
        BonusJewels = new List<int>() { 0, 0, 0 };
        ChangeState(GameState.StartDungeon);
    }

    #endregion


    #region 2. 던전 시작

    // 던전 시작 함수(서버에서만 실행 후, ClientRpc로 각 클라에 상태 업데이트)
    
    [Server]
    private void StartDungeon()
    {
        //Dungeon시작시 초기화
        CurrentStage = 0;
        //모든 클라 플레이어 UsedCards, Cards 초기화
        RpcInitPlayerUsedCards();

        //Dungeon시작시 변경
        CurrentDungeon++;
        this.Enqueue4MonstersFromData(_currentDungeon);

        //상태변화
        ChangeState(GameState.StartStage);
    }

    [ClientRpc]
    private void RpcInitPlayerUsedCards()
    {
        foreach (NetworkIdentity identity in NetworkClient.spawned.Values)
        {
            MyPlayer player = identity.GetComponent<MyPlayer>();
            if(player == null) continue;

            player.UsedCards = new List<int>();
            player.Cards = new List<int> {1,2,3,4,5,6,7};
        }
    }
    [ClientRpc]
    private void RpcInitPlayerCards()
    {

    }

    #endregion

    #region 2-1. 스테이지 시작
    [Server]
    private void StartStage()
    {
        //Stage시작시 초기화
        SubmittedCardList = new Dictionary<int, int>();
        DuplicationCheck = new Dictionary<int, int>();
        SelectedJewelIndexList = new Dictionary<uint, int>();
        NetIdAndJewelsIndex = new Dictionary<int, List<int>>();

        //Stage시작시 변경
        CurrentStage++;
        CurrentMonster = this.DequeueMonsterCurrentStage();
        CurrentMonsterId = CurrentMonster.DataId;

        //상태변화
        ChangeState(GameState.SubmitCard);
    }

    #endregion

    #region 2-2. 플레이어 카드 세팅

    private void SetLocalPopupSelect()
    {
        BattleUIManager.Instance.RequestUpdateSelectCard();
    }

    #endregion



    #region 3. 카드 제출 처리


    #region 카드 제출 후 토벌 성공/실패 결정 전 까지
    //플레이어의 카드 제출 처리(클라에서 요청후, 서버에서 처리)
    [Command(requiresAuthority = false)]
    public void CmdAddSubmittedCard_OnClick_Card(int player_netId, int cardNum)
    {
        if (SubmittedCardList.ContainsKey(player_netId))
        {
            SubmittedCardList[player_netId] = cardNum;
        }
        else
        {
            SubmittedCardList.Add(player_netId, cardNum);
        }
        Debug.Log($"Added- netId:{player_netId}, num:{SubmittedCardList[player_netId]}");

        ServerCheckAllPlayerSubmitted();
    }

    [Server]
    public void ServerCheckAllPlayerSubmitted()
    {
        if (ServerAllPlayersSubmitted())
        {
            ServerOnAllPlayersSubmitted();
        }
        else
        {
            Debug.Log("아직 제출안한 플레이어 있음.");
        }
    }

    [Server]
    private bool ServerAllPlayersSubmitted()
    {
        return NetworkServer.connections.Count == SubmittedCardList.Count;
    }

    [Server]
    private void ServerOnAllPlayersSubmitted()
    {
        ChangeState(GameState.CalculateResults);
    }

    [Server]
    private void ServerDecideStageResult()
    {
        int monsterHp = CurrentMonster.HP;
        int totalDamage = ServerCalculTotalDamage();
        bool StageClear = totalDamage >= monsterHp;

        ServerRequestSetUsedCard();

        if(StageClear) { ChangeState(GameState.GetJewels); }
        else { ChangeState(GameState.LoseJewels); }
    }

    [Server]
    private int ServerCalculTotalDamage()
    {
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

        foreach (var kvp in SubmittedCardList)
        {
            if (DuplicationCheck[kvp.Value] == 1)
            {
                totalDamage += kvp.Value;
            }
        }

        Debug.Log($"totalDamage == {totalDamage}");
        return totalDamage;
    }

    [Server]
    public void ServerRequestSetUsedCard()
    {
        //디버깅용
        foreach (var kv in SubmittedCardList)
        {
            Debug.Log($"netId:{kv.Key} Card:{kv.Value}");
        }

        //-1. 플레이어가 낸 카드들 UsedCard에 저장하고 갱신하기.
        foreach (var kv in SubmittedCardList)
        {
            RpcSetPlayerUsedCard(kv.Key, kv.Value);
        }
    }

    [ClientRpc]
    public void RpcSetPlayerUsedCard(int playerNetId, int usedCard)
    {
        MyPlayer player = GetPlayerFromNetId(playerNetId);
        if (player == null) { Debug.LogError("player == null"); return; }

        if (player.UsedCards == null) { Debug.LogError("player.UsedCards == null"); return; }
        var list = player.UsedCards;
        list.Add(usedCard);

        player.UsedCards = list;

        player.Cards.Remove(usedCard);
    }
    #endregion


    #region 토벌 성공
    [Server]
    public void ServerChooseRewardedPlayer()
    {
        //0. 중복 카드 제거
        RemoveDuplicatedCard();

        for (int reward_n = 0; reward_n < 3; reward_n++)
        {
            if (CurrentMonster.Reward[reward_n] == null)
                break;
            //1. 가장 작은 카드 낸 플레이어 NetId 구하기(그 후 Dic에서 삭제)
            //(1명으로 test하면 보상이 2개 이상일때 에러 발생)
            List<int> playerNetIds = GetMinCardPlayerNetIds();

            if (playerNetIds.Count > 1) 
            { Debug.LogError("MinCardPlayer > 1"); }

            SubmittedCardList.Remove(playerNetIds[0]);

            //2. 모든 클라의 해당 NetId가진 플레이어에 보상 주기.
            RpcPlayerGetReward(playerNetIds[0], reward_n);
        }

        //상태변화
        ChangeState(GameState.EndStage);
    }

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

    public MyPlayer GetPlayerFromNetId(int playerNetId)
    {   
        NetworkIdentity networkIdentity;
        if (NetworkClient.spawned.TryGetValue((uint)playerNetId, out networkIdentity))
        {
            MyPlayer player = networkIdentity.gameObject.GetComponent<MyPlayer>();
            return player;
        }
        else { return null; }
    }


    [ClientRpc]
    public void RpcPlayerGetReward(int playerNetId, int reward_n)
    {
        //2-1. NetId의 플레이어 찾기
        MyPlayer player = GetPlayerFromNetId(playerNetId);

        //2-2-1. 플레이어.Jewels에 대입할 List<int> 생성
        List<int> newJewels = player.Jewels;

        //2-2-2. newJewels에 해당 Reward의 보석 추가.
        if (CurrentMonster.Reward[reward_n] != null)
        {
            newJewels[0] += CurrentMonster.Reward[reward_n][0];
            newJewels[1] += CurrentMonster.Reward[reward_n][1];
            newJewels[2] += CurrentMonster.Reward[reward_n][2];
        }

        //2-2-3. player.Jewels에 대입(이래야 프로퍼티가 불린다)
        player.Jewels = newJewels;
    }
    #endregion


    #region 토벌 실패

    [Server]
    public void ServerChooseLoseJewelsPlayer()
    {
        //1. 가장 작은 카드를 낸 플레이어들의 NetId 구하기
        List<int> losePlayerNetIds = GetMinCardPlayerNetIds();

        //2. 해당 NetId의 플레이어가 가장 많이 가진 보석의 색깔 구하기.
        foreach (int netId in losePlayerNetIds)
        {
            MyPlayer player = GetPlayerFromNetId(netId);
            List<int> maxJewels = FindMaxIndexes(player.Jewels);
            NetIdAndJewelsIndex.Add(netId, maxJewels);
        }

        //3-1. 보석의 색깔이 여러개면, 플레이어에게 어떤 보석을 버릴지 선택을 시킨다
        //이거는 그냥 UI만 띄우는 거고, 실제 보석 버리기는 Content_Jewel을 통해 시킴.
        foreach(var kv in NetIdAndJewelsIndex)
        {
            RpcSetUIToLoseJewels(kv.Key, kv.Value);
        }
    }

    private List<int> FindMaxIndexes(List<int> list)
    {
        int maxValue = list.Max();
        List<int> maxIndexes = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == maxValue)
            {
                maxIndexes.Add(i);
            }
        }
        return maxIndexes;
    }

    //보석을 잃을 플레이어에게 UI 띄워주기
    [ClientRpc]
    private void RpcSetUIToLoseJewels(int playerId, List<int> maxJewels)
    {
        //모든 클라에 날려서 보석을 잃는 플레이어가 아니면 return
        if (NetworkClient.localPlayer.netId != playerId) { return; }

        //패배 플레이어에 잃을 보석을 선택하는 UI를 띄워준다.
        BattleUIManager.Instance.RequestUpdateRemoveJewels(maxJewels);

        //UI에서 Content_Jewel OnClick 시 보석 잃는 로직으로.
    }

    [Command(requiresAuthority = false)]
    public void CmdAddSelectedJewelIndexList_OnClick(uint playerNetId, int jewelIndex)
    {
        //중복 선택으로 패배하면 여기서 무슨일이 생긴다
        SelectedJewelIndexList.Add(playerNetId, jewelIndex);
    }

    //4. 패배 플레이어들이 선택 다 했는지 체크.
    [Command(requiresAuthority = false)]
    public void CmdCheckAllPlayerSelectedJewel_OnClick()
    {
        if (ServerAllPlayerSelectedJewel(NetIdAndJewelsIndex.Count))
        {
            //선택을 바탕으로 모든 클라에서 패배 플레이어의 Jewel 보너스로.
            ServerOnAllPlayersSelectedJewel();
        }
    }

    [Server]
    private bool ServerAllPlayerSelectedJewel(int dic_count)
    {
        return (dic_count == SelectedJewelIndexList.Count);
    }

    [Server]
    private void ServerOnAllPlayersSelectedJewel()
    {
        ServerAddJewelsToBonus();

        foreach(var kv in SelectedJewelIndexList)
        {
            //실제로 플레이어의 보석이 빠져나가는 부분.
            RpcPlayerLoseJewels((int)kv.Key, kv.Value);
        }

        //상태변화
        ChangeState(GameState.EndStage);
    }

    //Server의 보너스에 더하는 로직
    [Server]
    private void ServerAddJewelsToBonus()
    {
        List<int> newBonus = new List<int>();
        //그냥 newBonus에 BonusJewels를 대입하면 참조 복사가 일어나 값 변경시 hook이 발생하지 않음.
        BonusJewels.ForEach(item => newBonus.Add(item));

        foreach (var kv in SelectedJewelIndexList)
        {
            newBonus[kv.Value] += GetPlayerFromNetId((int)kv.Key).Jewels[kv.Value];
        }
        BonusJewels = newBonus;
    }

    //클라의 패배 플레이어 Jewel을 없애는 로직
    [ClientRpc]
    private void RpcPlayerLoseJewels(int netId, int jewelIndex)
    {
        MyPlayer player = GetPlayerFromNetId(netId);

        List<int> newJewels = player.Jewels;
        newJewels[jewelIndex] = 0;
        player.Jewels = newJewels;
    }

    #endregion


    #endregion

    #region EndStage
    [Server]
    private void ServerCheckStageOver()
    {
        if(CurrentDungeonMonsterQueue.Count > 0) { ChangeState(GameState.StartStage); }
        else { ChangeState(GameState.EndDungeon); }
    }
    #endregion

    #region EndDungeon
    [Server]
    private void ServerCheckDungeonOver()
    {
        if (CurrentDungeon == 3) { ChangeState(GameState.EndGame); }
        else { ChangeState(GameState.StartDungeon); }
    }
    #endregion

    #region EndGame
    //플레이어들이 가진 보석으로 점수 계산,
    //가장 점수가 높은 플레이어가 우승.
    //Lobby로 돌아옴.
    [Server]
    private void TempEndGame()
    {
        Debug.LogError("이제 가망이 없어.");
    }
    #endregion
}
