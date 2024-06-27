using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager : UIManager
{
    private void Start()
    {
        OpenSpecificUI(UIType.Battle);
        
    }
}
