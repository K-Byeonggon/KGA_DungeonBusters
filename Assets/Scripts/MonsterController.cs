using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterAnim
{
    Idle,
    Atk,
    Damage,
    Die,
    Win
}

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

    public void SetAnimator(MonsterAnim anim)
    {
        Animator animator = Monsters[currentId-1].GetComponent<Animator>();

        switch (anim)
        {
            case MonsterAnim.Idle:
                animator.SetBool("Die", false);
                break;
            case MonsterAnim.Atk:
                animator.SetTrigger("Atk");
                break;
            case MonsterAnim.Damage:
                animator.SetTrigger("Damage");
                break;
            case MonsterAnim.Die:
                animator.SetBool("Die", true);
                break;
            case MonsterAnim.Win:
                animator.SetTrigger("Win");
                break;
        }
    }
}
