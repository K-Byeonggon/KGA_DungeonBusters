using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUIManager : UIManager
{
    private void Start()
    {
        OpenSpecificUI(UIType.Login);
    }
}
