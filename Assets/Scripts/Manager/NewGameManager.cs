using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameManager : NetworkBehaviour
{
    public static NewGameManager Instance;

    //게임 상태 변수
    [SerializeField] int _currentDungeon;                               //현재 진행중인 던전
    [SerializeField] int _currentStage;                                 //현재 진행중인 스테이지
    [SerializeField] Monster _currentMonster;                           //현재 전투중인 몬스터
    [SerializeField] Queue<Monster> _currentDungeonMonsterQueue;        //현재 진행중인 던전에 있는 몬스터를 담은 Queue
                                                                        //그 외에 플레이어가 제출한 카드 등등이 있을 수 있다.
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



    #region 1. 게임 초기화

    //게임 초기화 함수(서버에서만 실행)
    [Server]
    private void InitializeGame()
    {
        CurrentDungeon = 0;
        CurrentStage = 0;
    }

    #endregion


    #region 2. 던전 시작

    // 던전 시작 함수(서버에서만 실행 후, ClientRpc로 각 클라에 상태 업데이트)
    [Server]
    private void StartDungeon()
    {
        CurrentDungeon++;
        this.Enqueue4MonstersFromData(_currentDungeon);

        RpcUpdateDungeonState(CurrentDungeon);
    }

    // 클라이언트에서 던전 상태 업데이트
    [ClientRpc]
    private void RpcUpdateDungeonState(int dungeon)
    {
        CurrentDungeon = dungeon;
    }

    #endregion

    #region 2-1. 스테이지 시작

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
