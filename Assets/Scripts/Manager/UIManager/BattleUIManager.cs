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
        battleUI.UpdateRewardsPanel();
    }


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

    public void RequestUpdateSelectCard()
    {
        battleUI.UpdateSelectCardPopup();
    }

    public void RequestUpdateRemoveJewels(List<int> maxJewels)
    {
        battleUI.UpdateRemoveJewels(maxJewels);
    }

    public void RequestUpdateBonusJewels()
    {
        battleUI.UpdateBonusJewels();
    }
}
