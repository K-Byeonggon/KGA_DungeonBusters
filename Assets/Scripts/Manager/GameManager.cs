using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [SerializeField] int _currentDungeon;
    [SerializeField] int _currentStage;
    [SyncVar(hook = nameof(OnCurrentMonsterChanged))][SerializeField] Monster _currentMonster;

    public Queue<Monster> _monsterQueue;

    public int CurrentDungeon
    {
        get { return _currentDungeon; }
        set 
        {
            _currentDungeon = value;
            //Dungeon�� �ٲ�� �ٷ� �� ������ ���� 4������ �����Ѵ�.
            if (isServer)
            {
                Get4MonstersFromData(CurrentDungeon);
            }
        }
    }

    public int CurrentStage
    {
        get { return _currentStage; }
        set
        {
            _currentStage = value;
            
        }
    }

    public Monster CurrentMonster
    {
        get { return _currentMonster; }
        set
        {
            if (isServer)
            {
                _currentMonster = value;
                //currentMonster�� ����Ǹ� �ٷ� UI�� ǥ���ϵ��� ����.
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        CurrentDungeon = 1;
        CurrentStage = 1;

        if (_monsterQueue.Count > 0)
        {
            CurrentMonster = _monsterQueue.Dequeue();
        }
    }

    private void OnCurrentMonsterChanged(Monster oldMonster, Monster newMonster)
    {
        //���⼭ Ŭ���̾�Ʈ UI�� ������Ʈ�ϼ���.
    }

    [Server]
    public void Get4MonstersFromData(int dungeon)
    {
        _monsterQueue = new Queue<Monster>();
        List<int> randomList = new List<int>() { 1,2,3,4,5 };
        
        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, randomList.Count);
            int randomNum = randomList[randomIndex] + ((dungeon - 1) * 5);
            randomList.RemoveAt(randomIndex);
            Monster monster = DataManager.Instance.GetMonster(randomNum);
            _monsterQueue.Enqueue(monster);
        }
    }

    [Command]
    public void CmdSetNextMonster()
    {
        if(_monsterQueue.Count > 0)
        {
            CurrentMonster = _monsterQueue.Dequeue();
        }
    }
}
