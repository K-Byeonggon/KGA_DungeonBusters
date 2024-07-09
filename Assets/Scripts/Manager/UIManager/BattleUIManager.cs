using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager : UIManager
{
    public BattleUI battleUI;
    public static new BattleUIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OpenSpecificUI(UIType.Battle);
        battleUI = GetCreatedUI(UIType.Battle).GetComponent<BattleUI>();
    }

    #region BattleUI
    public void RequestUpdateDungeon()
    {
        battleUI.UpdateDungeon();
    }

    public void RequestUpdateStage()
    {
        battleUI.UpdateStage();
    }

    public void RequestUpdateMonster()
    {
        battleUI.UpdateEnemyPanel();
        battleUI.UpdateRewardsPanel();  //Panel_Rewards
    }

    public void RequestUpdateBonusJewels()
    {
        battleUI.UpdateBonusJewels();
    }
    #endregion

    #region Panel_Players
    public void RequestCreatePlayer(int player_netId)
    {
        battleUI.RequestCreatePlayerPanel(player_netId);
    }

    public void RequestUpdatePlayerJewels(int player_netId)
    {
        battleUI.RequestUpdatePlayerJewels(player_netId);
    }

    public void RequestUpdatePlayerUsedCards(int player_netId)
    {
        battleUI.RequestUpdatePlayerUsedCards(player_netId);
    }
    #endregion

    #region Popup_Select
    public void RequestUpdateSelectCard()
    {
        battleUI.UpdateSelectCardPopup();
    }
    #endregion

    #region Popup_Remove
    public void RequestUpdateRemoveJewels(List<int> maxJewels)
    {
        battleUI.UpdateRemoveJewels(maxJewels);
    }
    #endregion

    #region Popup_GetBonus
    public void RequestUpdateGetBonus()
    {
        battleUI.UpdateGetBonus();
    }

    public void RequestSetGetBonus(int playerNetId)
    {
        battleUI.SetGetBonus(playerNetId);
    }

    public void RequestUnsetGetBonus()
    {
        battleUI.UnsetGetBonus();
    }
    #endregion

    public void OpenEndGame()
    {
        CloseSpecificUI(UIType.Battle);
        OpenSpecificUI(UIType.Point);
    }
}
