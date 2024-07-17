using Mirror;
using System.Collections;
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
                                                                        //�� �ܿ� �÷��̾ ������ ī�� ����� ���� �� �ִ�.
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
            BattleUIManager.Instance.RequestUpdateMonster();
        }
    }
    
    public Queue<Monster> CurrentDungeonMonsterQueue
    {
        get { return _currentDungeonMonsterQueue; }
        set { _currentDungeonMonsterQueue = value; }
    }

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

        RpcUpdateDungeonState(CurrentDungeon);

        ChangeState(GameState.StartStage);
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

    [Server]
    private void StartStage()
    {
        CurrentStage++;
        this.DequeueMonsterCurrentStage();

        RpcUpdateStageState(CurrentStage, CurrentMonster);

        ChangeState(GameState.SubmitCard);
    }

    [ClientRpc]
    private void RpcUpdateStageState(int stage, Monster monster)
    {
        Debug.Log("ClientRpc: UpdateStageState");
        CurrentStage = stage;
        CurrentMonster = monster;
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
