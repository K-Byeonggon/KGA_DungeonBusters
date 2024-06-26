using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [Header("Panel_Enemy")]
    [SerializeField] Text Text_MonsterName;
    [SerializeField] Slider Slider_Hp;
    [SerializeField] Text Text_Hp;

    [Header("Panel_Dungeon")]
    [SerializeField] Text Text_CurrentDungeon;
    [SerializeField] Text Text_CurrentStage;

    [Header("Panel_Button")]
    [SerializeField] Button Btn_SelectCard;

    private void Start()
    {
        SetEnemy();
        SetDungeon();
    }


    public void SetEnemy()
    {
        var monster = GameManager.Instance.CurrentMonster;
        Text_MonsterName.text = monster.Name;
        Text_Hp.text = $"{monster.HP}";
    }

    public void SetDungeon()
    {
        Text_CurrentDungeon.text = $"Dungeon {GameManager.Instance.CurrentDungeon}";
        Text_CurrentStage.text = $"{GameManager.Instance.CurrentStage}/4";
    }

    public void OnClick_SelectCard()
    {

    }


}
