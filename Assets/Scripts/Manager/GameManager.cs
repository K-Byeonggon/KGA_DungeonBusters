using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] int _currentDungeon;
    [SerializeField] int _currentStage;
    [SerializeField] Monster _currentMonster;

    public Queue<Monster> _monsterQueue;

    public int CurrentDungeon
    {
        get { return _currentDungeon; }
        set 
        {
            _currentDungeon = value;
            //Dungeon�� �ٲ�� �ٷ� �� ������ ���� 4������ �����Ѵ�.
            Get4MonstersFromData(CurrentDungeon);
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
            _currentMonster = value;
            //currentMonster�� ����Ǹ� �ٷ� UI�� ǥ���ϵ��� ����.
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        CurrentDungeon = 1;
        CurrentStage = 1;

        CurrentMonster = _monsterQueue.Dequeue();

    }

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


}
