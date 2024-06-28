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

    public void RequestUpdatePlayer()
    {

    }
}
