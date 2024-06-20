using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomUIManager : UIManager
{
    private void Start()
    {
        OpenSpecificUI(UIType.Room);
    }
}
