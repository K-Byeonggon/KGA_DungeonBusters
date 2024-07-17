using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewGameManager : NetworkBehaviour
{
    public static NewGameManager Instance;

    //���� ���� ����
    [SerializeField] GameState _currentState;
    [SerializeField] int _currentDungeon;                               //���� �������� ����
    [SerializeField] int _currentStage;                                 //���� �������� ��������
    [SerializeField] Monster _currentMonster;                           //���� �������� ����
    [SerializeField] Queue<Monster> _currentDungeonMonsterQueue;        //���� �������� ������ �ִ� ���͸� ���� Queue
    [SerializeField] Dictionary<int, int> _submittedCardList;           //key:netId, value:������ ī��Num
    [SerializeField] Dictionary<int, int> _duplicationCheck;            //key:CardNum, value:�ش�Num�� ����
    [SerializeField] List<int> _bonusJewels;                            //���ʽ� Jewel
    [SerializeField] Dictionary<uint, int> _selectedJewelIndexList;     //key:netId, value:�÷��̾ ������ ���� Jewel �ε���
    [SerializeField] Dictionary<int, List<int>> _netIdAndJewelsIndex;    //key:netId, value:���帹��Jewel�� �ε���List

    [SerializeField] int _currentMonsterId;

    public Dictionary<int, Monster> _monsterList = new Dictionary<int, Monster>();

    #region ������Ƽ
    public int CurrentDungeon
    {
        get { return _currentDungeon; }
        set
        {
            _currentDungeon = value;
            //���⼭ UI ����? �ϸ� Ŭ�� ���� ������״� UI ���� �ǰ���?
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



    #region ���¸ӽ�
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




    #region 1. ���� �ʱ�ȭ

    //���� �ʱ�ȭ �Լ�(���������� ����)
    [Server]
    private void InitializeGame()
    {
        CurrentDungeon = 0;
        CurrentStage = 0;
        SubmittedCardList = new Dictionary<int, int>();
        DuplicationCheck = new Dictionary<int, int>();
        BonusJewels = new List<int>() { 0, 0, 0 };
        SelectedJewelIndexList = new Dictionary<uint, int>();
        NetIdAndJewelsIndex = new Dictionary<int, List<int>>();
        ChangeState(GameState.StartDungeon);
    }

    #endregion


    #region 2. ���� ����

    // ���� ���� �Լ�(���������� ���� ��, ClientRpc�� �� Ŭ�� ���� ������Ʈ)
    
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

    // Ŭ���̾�Ʈ���� ���� ���� ������Ʈ
    [ClientRpc]
    private void RpcUpdateDungeonState(int dungeon)
    {
        Debug.Log("ClientRpc: UpdateDungeonState");
        CurrentDungeon = dungeon;
    }

    #endregion

    #region 2-1. �������� ����

    //�������� ������ �ʱ�ȭ: �÷��̾���� ������ ī��, �÷��̾� ��������, ����� ī��

    [Server]
    private void StartStage()
    {
        CurrentStage++;
        this.DequeueMonsterCurrentStage();

        //CurrentMonster = DataManager.Instance.GetMonster(0);
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
    //StartServer���� Server�� �� �ٲٰ�, StartClient���� Ŭ�� ����
    [Command(requiresAuthority = false)]
    private void CmdSendAllStateToClient()
    {
        RpcUpdateDungeonState(CurrentDungeon);
        //���⼭ ����ID�� �ָ�, Ŭ�󿡼� �ٽ� xml���� Id�� ���� ���� �ޱ�.
        RpcUpdateStageState(CurrentStage, CurrentMonster.DataId);
    }

    #endregion

    #region 2-2. �÷��̾� ī�� ����

    private void SetLocalPopupSelect()
    {
        BattleUIManager.Instance.RequestUpdateSelectCard();
    }

    #endregion



    #region 3. ī�� ���� ó��

    // �̰� ��� Ŭ�󿡼� ī�� ���� �ϴ°ǰ�?
    public void OnSubmitCard(int card)
    {
        if (isClient)
        {
            //CmdSubmitCard(NetworkClient.connection.identity.GetComponent<MyPlayer>().playerId, card);
        }
    }

    //�÷��̾��� ī�� ���� ó��(Ŭ�󿡼� ��û��, �������� ó��)
    [Command(requiresAuthority = false)]
    public void CmdAddSubmittedCard(int netId, int cardNum)
    {
        int playerNetId = netId;

        if (SubmittedCardList.ContainsKey(playerNetId))
        {
            SubmittedCardList[playerNetId] = cardNum;
        }
        else
        {
            SubmittedCardList.Add(playerNetId, cardNum);
        }
        Debug.Log($"Added- netId:{playerNetId}, num:{SubmittedCardList[playerNetId]}");

        CmdCheckAllPlayerSubmitted();
    }

    //[Command(requiresAuthority = false)]
    [Server]
    public void CmdCheckAllPlayerSubmitted()
    {
        if (AllPlayersSubmitted())
        {
            OnAllPlayersSubmitted();
        }
        else
        {
            Debug.Log("���� ������� �÷��̾� ����.");
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

        CmdRequestSetUsedCard();

        Debug.Log($"totalDamage == {totalDamage}");
        if(StageClear) { CmdChooseRewardedPlayer(); }
        else { CmdPutJewelsInBonus(); }
    }



    #region ���� ���� ����
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


    //��� �����ô� ������ ���� ���� ���� ���� �������� ��찡 ������,
    //��� ���нô� ������ ���� ���� ���� ���� �������� �� �ִ�.
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

    //[Command(requiresAuthority = false)]
    [Server]
    public void CmdRequestSetUsedCard()
    {
        //������
        foreach(var kv in SubmittedCardList)
        {
            Debug.Log($"netId:{kv.Key} Card:{kv.Value}");
        }

        //-1. �÷��̾ �� ī��� UsedCard�� �����ϰ� �����ϱ�.
        foreach (var kv in SubmittedCardList)
        {
            RpcSetPlayerUsedCard(kv.Key, kv.Value);
        }
    }

    //[Command(requiresAuthority = false)]
    [Server]
    public void CmdChooseRewardedPlayer()
    {
        //0. �ߺ� ī�� ����
        RemoveDuplicatedCard();
        
        for (int reward_n = 0; reward_n < 3; reward_n++)
        {
            if (CurrentMonster.Reward[reward_n] == null)
                break;
            //1. ���� ���� ī�� �� �÷��̾� NetId ���ϱ�(�� �� Dic���� ����)
            //(1������ test�ϸ� ������ 2�� �̻��϶� ���� �߻�)
            List<int> playerNetIds = GetMinCardPlayerNetIds();
            int usedCard = SubmittedCardList[playerNetIds[0]];
            SubmittedCardList.Remove(playerNetIds[0]);


            //2. ��� Ŭ���� �ش� NetId���� �÷��̾ ���� �ֱ�.
            foreach (int playerNetId in playerNetIds)
            {
                RpcPlayerGetReward(playerNetId, reward_n);
            }

            
        }
    }

    [ClientRpc]
    public void RpcPlayerGetReward(int playerNetId, int reward_n)
    {
        //2-1. NetId�� �÷��̾� ã��
        MyPlayer player = GetPlayerFromNetId(playerNetId);

        //2-2. �÷��̾ �ش� Reward�� ���� �߰�.
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
        if(player == null) { Debug.LogError("player == null"); return; }

        if (player.UsedCards == null) { Debug.LogError("player.UsedCards == null"); return; }
        player.UsedCards.Add(usedCard);


        player.Cards.Remove(usedCard);
    }

    #endregion

    #region ���� �Ҵ� ����

    [Command(requiresAuthority = false)]
    public void CmdPutJewelsInBonus()
    {
        //1. ���� ���� ī�带 �� �÷��̾���� NetId ���ϱ�
        List<int> losePlayerNetIds = GetMinCardPlayerNetIds();

        //2. �ش� NetId�� �÷��̾ ���� ���� ���� ������ ���� ���ϱ�.
        foreach (int netId in losePlayerNetIds)
        {
            MyPlayer player = GetPlayerFromNetId(netId);
            List<int> maxJewels = FindMaxIndexes(player.Jewels);
            NetIdAndJewelsIndex.Add(netId, maxJewels);
        }

        //3-1. ������ ������ ��������, �÷��̾�� � ������ ������ ������ ��Ų��
        //�׳� ���� ���� �ϳ��� �÷��̾ Ŭ���ϰ� ����. �׷��� �ϰ������� ClientRpc������ �ȴ�.
        foreach(var kv in NetIdAndJewelsIndex)
        {
            RpcSetUIToLoseJewels(kv.Key, kv.Value);
        }

        


    }

    //4. �й� �÷��̾���� ���� �� �ߴ��� üũ.
    public void RemoveJewelsAndSetBonus()
    {
        if (AllPlayerSelectedJewel(NetIdAndJewelsIndex.Count))
        {
            //������ �������� ��� Ŭ�󿡼� �й� �÷��̾��� Jewel ���ʽ���.
            OnAllPlayersSelectedJewel();
        }
    }

    private bool AllPlayerSelectedJewel(int dic_count)
    {
        return (dic_count == SelectedJewelIndexList.Count);
    }

    private void OnAllPlayersSelectedJewel()
    {
        AddJewelsToBonus();

        RpcLoseJewels();

        CmdRequestUpdateBonus();
    }

    //Server�� ���ʽ��� ���ϴ� ����
    [Server]
    private void AddJewelsToBonus()
    {
        foreach (var kv in SelectedJewelIndexList)
        {
            BonusJewels[kv.Value] += GetPlayerFromNetId((int)kv.Key).Jewels[kv.Value];
        }
    }

    //Ŭ���� �й� �÷��̾� Jewel�� ���ִ� ����
    [ClientRpc]
    private void RpcLoseJewels()
    {
        foreach (var kv in SelectedJewelIndexList)
        {
            GetPlayerFromNetId((int)kv.Key).Jewels[kv.Value] = 0;
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdRequestUpdateBonus()
    {
        UpdateBonus(BonusJewels);
    }

    [ClientRpc]
    private void UpdateBonus(List<int> bonus)
    {
        BonusJewels = bonus;
    }

    //������ ���� �÷��̾�� UI ����ֱ�
    [ClientRpc]
    private void RpcSetUIToLoseJewels(int playerId, List<int> maxJewels)
    {
        //��� Ŭ�� ������ ������ �Ҵ� �÷��̾ �ƴϸ� return
        if(NetworkClient.localPlayer.netId != playerId) { return; }

        //�ϴ� ���� ����â �������
        BattleUIManager.Instance.RequestUpdateRemoveJewels(maxJewels);

        //�״�������? �ϴ� �÷��̾ ����â���� ������ �ϰ���?
    }

    private List<int> FindMaxIndexes(List<int> list)
    {
        int maxValue = list.Max();
        List<int> maxIndexes = new List<int>();
        for(int i = 0; i < list.Count; i++)
        {
            if (list[i] == maxValue)
            {
                maxIndexes.Add(i);
            }
        }
        return maxIndexes;
    }

    #endregion


    #endregion


    #region 4. ���� ��� ��� �� ���� �й�
    //���� ��� ��� �� ���� �й� (�������� ���� ��, ClientRpc�� �� Ŭ�� ���� ������Ʈ)
    [Server]
    private void CalculateBattleResult()
    {
        //���� ��� ��� �޼���
        //���� �й� �޼���
        RpcUpdateGameState();
    }

    //���� ����ȭ �Լ�
    [ClientRpc]
    private void RpcUpdateGameState()
    {
        //Ŭ���̾�Ʈ�� ���� ���� ������Ʈ
    }
    #endregion


}
