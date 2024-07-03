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

    [Header("Panel_Bonus")]
    [SerializeField] Text Text_BonusRed;
    [SerializeField] Text Text_BonusYellow;
    [SerializeField] Text Text_BonusBlue;


    //이렇게 가지고 있는 방법 말고 다른 방법이 있을 것이다.
    [Header("Panels")]
    [SerializeField] Panel_Players panel_Players;
    [SerializeField] Panel_Rewards panel_Rewards;

    [Header("Popups")]
    [SerializeField] Popup_Select popup_Select;
    [SerializeField] Popup_RemoveJewels popup_RemoveJewels;

    public void RequestCreatePlayerPanel(int player_netId)
    {  
        panel_Players.CreatePlayerPanel(player_netId);
    }

    public void RequestUpdatePlayerPanel(int player_netId)
    {
        panel_Players.UpdatePlayerPanel(player_netId);
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
        panel_Rewards.RemoveRewards();
        panel_Rewards.SetRewardUI();
    }

    public void UpdateSelectCardPopup()
    {
        popup_Select.RemoveCards();
        popup_Select.SetCards();
    }

    public void UpdateBonusJewels()
    {
        Text_BonusRed.text = $"{NewGameManager.Instance.BonusJewels[0]}";
        Text_BonusYellow.text = $"{NewGameManager.Instance.BonusJewels[1]}";
        Text_BonusBlue.text = $"{NewGameManager.Instance.BonusJewels[2]}";
    }

    public void UpdateRemoveJewels(List<int> maxJewels)
    {
        popup_RemoveJewels.RemoveJewels();
        popup_RemoveJewels.SetJewels(maxJewels);
        popup_RemoveJewels.SetBonus();
        popup_RemoveJewels.UISetActive(true);
    }

    public void OnClick_SelectCard()
    {
        popup_Select.gameObject.SetActive(true);
    }


}
