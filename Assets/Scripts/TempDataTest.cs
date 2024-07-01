using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDataTest : MonoBehaviour
{
    Monster _testMonster;

    // Start is called before the first frame update
    void Start()
    {
        _testMonster = DataManager.Instance.GetMonster(1);

        Debug.Log($"{_testMonster.Name}, {_testMonster.HP}");

        foreach (var reward in _testMonster.Reward[0])
        {
            Debug.Log($"{reward}");
        }
    }

}
