using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameManager : NetworkBehaviour
{
    public static NewGameManager Instance;

    //���� ���� ����
    [SerializeField] int _currentDungeon;                               //���� �������� ����
    [SerializeField] int _currentStage;                                 //���� �������� ��������
    [SerializeField] Monster _currentMonster;                           //���� �������� ����
    [SerializeField] Queue<Monster> _currentDungeonMonsterQueue;        //���� �������� ������ �ִ� ���͸� ���� Queue
                                                                        //�� �ܿ� �÷��̾ ������ ī�� ����� ���� �� �ִ�.
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
        set { _currentMonster = value; }
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



    #region 1. ���� �ʱ�ȭ

    //���� �ʱ�ȭ �Լ�(���������� ����)
    [Server]
    private void InitializeGame()
    {
        CurrentDungeon = 0;
        CurrentStage = 0;
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
    }

    // Ŭ���̾�Ʈ���� ���� ���� ������Ʈ
    [ClientRpc]
    private void RpcUpdateDungeonState(int dungeon)
    {
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
    }

    [ClientRpc]
    private void RpcUpdateStageState(int stage, Monster monster)
    {
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
