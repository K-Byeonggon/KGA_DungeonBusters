using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private List<GameObject> Monsters;
    private int currentId;

    private void Start()
    {
        currentId = 0;
        Monsters = new List<GameObject>();
        Transform parent = this.transform;
        foreach(Transform child in parent)
        {
            Monsters.Add(child.gameObject);
        }
    }

    public void SetActiveMonster(int monsterId)
    {
        currentId = monsterId;
        Monsters[currentId - 1].SetActive(true);
        
    }

    public void UnsetMonster()
    {
        if(currentId != 0) Monsters[currentId -1].SetActive(false);
    }
}
