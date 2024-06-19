using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : UIManager
{
    private void Start()
    {
        OpenSpecificUI(UIType.Lobby);
    }
}
