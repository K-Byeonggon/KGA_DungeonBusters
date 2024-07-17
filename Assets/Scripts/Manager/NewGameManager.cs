using Mirror;
using System.Collections.Generic;
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
    [SerializeField] Dictionary<int, int> _submittedCardList;

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
    }



    [Command]
    public void CmdSubmitCard(int playerId, int card)
    {
        if (!isServer) return;

        HandleCardSubmission(playerId, card);
    }

    [Server]
    private void HandleCardSubmission(int playerId, int card)
    {
        CalculateBattleResult();
    }

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
