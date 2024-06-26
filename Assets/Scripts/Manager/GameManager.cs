using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int _currentStage;

    public Queue<Monster> _monsterQueue;

    public int CurrentStage
    {
        get { return _currentStage; }
        set 
        {
            _currentStage = value;
            //stage set될때 일어나는 것들.
        }
    }

    private void OnEnable()
    {
        CurrentStage = 2;
        Get4MonstersFromData(CurrentStage);

        foreach (Monster monster in _monsterQueue)
        {
            Debug.Log($"{monster.Name} + {monster.Stage}");
        }
    }

    public void Get4MonstersFromData(int stage)
    {
        _monsterQueue = new Queue<Monster>();
        List<int> randomList = new List<int>() { 1,2,3,4,5 };
        
        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, randomList.Count);
            int randomNum = randomList[randomIndex] + ((stage - 1) * 5);
            randomList.RemoveAt(randomIndex);
            Monster monster = DataManager.Instance.GetMonster(randomNum);
            _monsterQueue.Enqueue(monster);
        }
    }

    


    public Monster DequeueMonster()
    {
        return _monsterQueue.Dequeue();
    }
}
