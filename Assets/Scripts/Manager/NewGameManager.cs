using Mirror;
using System.Collections;
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
    [SerializeField] List<int> _winPlayerIds;
    private int _currentSelectBonusPlayerIndex;

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

    public List<int> WinPlayerIds
    {
        get { return _winPlayerIds; }
        set { _winPlayerIds = value; }
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
        BattleUIManager.Instance.RequestUpdateGetBonus();
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

        OnStateEnter(_currentState);
        //StartCoroutine(ChangeStateCoroutine());
    }

    private IEnumerator ChangeStateCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        OnStateEnter(_currentState);
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
                //SetLocalPopupSelect();
                break;
            case GameState.CalculateResults:
                ServerDecideStageResult();
                break;
            case GameState.GetJewels:
                ServerChooseRewardedPlayer();
                break;
            case GameState.GetBonus:
                ServerStartGetBonusProcess();
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
            if(MyNetworkRoomManager.Instance.minPlayers < 4) { player.Cards = new List<int>() { 2,3,4,5,6,7}; }
            else { player.Cards = new List<int>() { 1, 2, 3, 4, 5, 6 }; }
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
        WinPlayerIds = new List<int>();

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
        bool dungeonStart = (CurrentDungeonMonsterQueue.Count == 3);

        //이 부분은 게임 시작할때는 플레이어 못찾아서 세팅 안됨. 그래서 MyPlayer에서 한번더 부르고 있음.
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
        var newUsedCards = player.UsedCards;
        newUsedCards.Add(usedCard);
        player.UsedCards = newUsedCards;

        if(player.Cards == null) { Debug.LogError("player.Cards == null"); return; }
        var newCards = player.Cards;
        newCards.Remove(usedCard);
        player.Cards = newCards;
    }
    #endregion


    #region GetJewels
    [Server]
    public void ServerChooseRewardedPlayer()
    {
        //0. SubmittedCardList에서 중복 카드 제거
        RemoveDuplicatedCard();

        //1. 승리자 List 만들기(작은 카드를 낸 플레이어 순서대로 Id저장)
        int whileCount = 0;
        while (SubmittedCardList.Count > 0)
        {
            List<int> minCardPlayerId = GetMinCardPlayerNetIds();
            if (minCardPlayerId.Count > 1) { Debug.LogError("DuplicatedCard Exists"); }
            WinPlayerIds.Add(minCardPlayerId[0]);
            SubmittedCardList.Remove(minCardPlayerId[0]);

            whileCount++;
            if (whileCount > 100) { Debug.LogError("while > 100"); break; }
        }

        //2. 승리자 List를 토대로 보상 분배
        List<int> rewardedPlayerIds = new List<int>();
        List<int> rewardIndexs = new List<int>();

        for (int i = 0; i < 3; i++)
        {
            if (CurrentMonster.Reward[i] == null)
                break;

            rewardedPlayerIds.Add(WinPlayerIds[i]);
            rewardIndexs.Add(i);
        }

        //3. 모든 클라의 해당 NetId가진 플레이어에 reward 주기
        int curMonsterId = CurrentMonster.DataId;
        RpcDistributeRewards(rewardedPlayerIds.ToArray(), rewardIndexs.ToArray(), curMonsterId);

        //상태변화
        bool isBonusEmpty = new HashSet<int>(BonusJewels).SetEquals(new List<int>() { 0, 0, 0});
        if (isBonusEmpty) { ChangeState(GameState.EndStage); }
        else { ChangeState(GameState.GetBonus); }
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
    private void RpcDistributeRewards(int[] playerIds, int[] rewardIndexs, int curMonsterId)
    {
        for(int i = 0; i < playerIds.Length; i++)
        {
            int playerId = playerIds[i];
            int rewardIndex = rewardIndexs[i];

            PlayerGetReward(playerId, rewardIndex, curMonsterId);
        }    
    }

    private void PlayerGetReward(int playerNetId, int reward_n, int curMonsterId)
    {
        //2-1. NetId의 플레이어 찾기
        MyPlayer player = GetPlayerFromNetId(playerNetId);

        //2-2-1. 플레이어.Jewels에 대입할 List<int> 생성
        List<int> newJewels = player.Jewels;

        //몬스터 꺼내서
        Monster curMonster = DataManager.Instance.GetMonster(curMonsterId);

        //2-2-2. newJewels에 해당 Reward의 보석 추가.
        if (curMonster.Reward[reward_n] != null)
        {
            newJewels[0] += curMonster.Reward[reward_n][0];
            newJewels[1] += curMonster.Reward[reward_n][1];
            newJewels[2] += curMonster.Reward[reward_n][2];
        }

        //2-2-3. player.Jewels에 대입(이래야 프로퍼티가 불린다)
        player.Jewels = newJewels;
    }
    #endregion


    #region GetBonus
    [Server]
    private void ServerStartGetBonusProcess()
    {
        _currentSelectBonusPlayerIndex = 0;
        RpcSetUIBonusSelect(WinPlayerIds[_currentSelectBonusPlayerIndex]);
    }

    [ClientRpc]
    private void RpcSetUIBonusSelect(int playerNetId)
    {
        //모든 플레이어에게 Popup_GetBonus 띄운다.
        //보너스 선택 플레이어가 아니면 WaitForSelect로 화면 바꾼다.
        //BattleUIManager.Instance.RequestUpdateGetBonus(playerNetId);

        BattleUIManager.Instance.RequestSetGetBonus(playerNetId);

        //선택은 Content_BonusJewel에서 이뤄지겠지?
    }

    [Command(requiresAuthority = false)]
    public void CmdSubBonusJewel_OnClick(int playerNetId, int jewelIndex)
    {
        List<int> newBonus = new List<int>();
        //그냥 newBonus에 BonusJewels를 대입하면 참조 복사가 일어나 값 변경시 hook이 발생하지 않음.
        BonusJewels.ForEach(item => newBonus.Add(item));
        newBonus[jewelIndex]--;
        BonusJewels = newBonus;

        //모든 클라의 해당 NetID플레이어에 Jewel 더하기
        RpcAddjewelToPlayer(playerNetId, jewelIndex);
    }

    [ClientRpc]
    private void RpcAddjewelToPlayer(int playerNetId, int jewelIndex)
    {
        MyPlayer player = GetPlayerFromNetId(playerNetId);

        List<int> newJewels = player.Jewels;
        newJewels[jewelIndex]++;
        player.Jewels = newJewels;
    }

    [Command(requiresAuthority = false)]
    public void CmdCheckAllBonusDistributed_OnClick()
    {
        //Content_BonusJewel에서 클릭후 NewGameManager의 모든 BonusJewles가 배분되었는지 확인.
        //배분 완료되면, 다음 State로
        //배분이 덜 끝났으면, 다음 사람에게 UI띄운다.
        _currentSelectBonusPlayerIndex = (_currentSelectBonusPlayerIndex+1) % WinPlayerIds.Count;

        //여기서 모든 클라의 GetBonusUI 꺼주자.
        RpcUnsetUIGetBonus();

        bool isBonusEmpty = new HashSet<int>(BonusJewels).SetEquals(new List<int>() { 0, 0, 0 });
        if(isBonusEmpty)
        {
            ChangeState(GameState.EndStage);
        }
        else
        {
            RpcSetUIBonusSelect(WinPlayerIds[_currentSelectBonusPlayerIndex]);
        }
    }

    [ClientRpc]
    private void RpcUnsetUIGetBonus()
    {
        BattleUIManager.Instance.RequestUnsetGetBonus();
    }

    #endregion


    #region LoseJewels

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
        RpcRequestOpenPointUI();
    }
    #endregion

    [ClientRpc]
    private void RpcRequestOpenPointUI()
    {
        BattleUIManager.Instance.OpenEndGame();
    }
}
