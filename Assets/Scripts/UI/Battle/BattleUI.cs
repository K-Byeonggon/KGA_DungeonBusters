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

    //이렇게 가지고 있는 방법 말고 다른 방법이 있을 것이다.
    [Header("Panels")]
    [SerializeField] Panel_Players panel_Players;
    [SerializeField] Panel_Rewards panel_Rewards;
    
    private void Start()
    {

    }


    public void RequestCreatePlayerPanel(int player_netId)
    {  
        panel_Players.CreatePlayerPanel(player_netId);
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


    public void UpdateEnemyPanel()
    {
        var monster = NewGameManager.Instance.CurrentMonster;
        Text_MonsterName.text = monster.Name;
        Text_Hp.text = $"{monster.HP}";
    }

    public void UpdateDungeon()
    {
        Text_CurrentDungeon.text = $"Dungeon {NewGameManager.Instance.CurrentDungeon}";
    }

    public void UpdateStage()
    {
        Text_CurrentStage.text = $"{NewGameManager.Instance.CurrentStage}/4";
    }

    public void UpdateRewardsPanel()
    {
        panel_Rewards.SetRewardUI();
    }

    public void OnClick_SelectCard()
    {

    }


}
