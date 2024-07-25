using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager : UIManager
{
    private BattleUI battleUI;
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

    public void RequestSetStageInfo(bool setActive)
    {
        battleUI.SetActiveStageInfo(setActive);
    }
    #endregion

    #region Panel_Players
    public void RequestCreatePlayer(uint player_netId)
    {
        battleUI.RequestCreatePlayerPanel(player_netId);
    }

    public void RequestUpdatePlayerJewels(uint player_netId)
    {
        battleUI.RequestUpdatePlayerJewels(player_netId);
    }

    public void RequestUpdatePlayerUsedCards(uint player_netId)
    {
        battleUI.RequestUpdatePlayerUsedCards(player_netId);
    }
    #endregion

    #region Popup_Select
    public void RequestUpdateSelectCard()
    {
        battleUI.UpdateSelectCardPopup();
    }

    public void RequestUnsetSelectCard()
    {
        battleUI.OnAllPlayerSelectedCard();
    }
    #endregion

    #region Popup_Remove
    public void RequestUpdateRemoveJewels(bool isLosePlayer)
    {
        battleUI.UpdateRemoveJewels(isLosePlayer);
    }

    public void RequestUnsetRemoveJewels()
    {
        battleUI.UnsetRemoveJewels();
    }
    #endregion

    #region Popup_GetBonus
    public void RequestUpdateGetBonus()
    {
        battleUI.UpdateGetBonus();
    }

    public void RequestSetGetBonus(uint playerNetId)
    {
        battleUI.SetGetBonus(playerNetId);
    }

    public void RequestUnsetGetBonus()
    {
        battleUI.UnsetGetBonus();
    }
    #endregion

    #region Popup_WinLose
    public void RequestUpdateWinLoseText()
    {
        battleUI.UpdateText();
    }

    public void RequestUpdateWinLoseCards()
    {
        battleUI.UpdateWinLoseCards();
    }

    public void RequestSetWinLose(bool setActive)
    {
        battleUI.SetWinLose(setActive);
    }
    #endregion

    public void OpenEndGame()
    {
        CloseSpecificUI(UIType.Battle);
        OpenSpecificUI(UIType.Point);
    }
}
