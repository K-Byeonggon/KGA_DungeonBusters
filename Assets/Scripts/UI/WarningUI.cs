using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningUI : MonoBehaviour
{
    [SerializeField] Button Btn_Okay;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Okay.onClick.AddListener(OnClick_Okay);
    }

    private void OnClick_Okay()
    {
        UIManager.Instance.CloseSpecificUI(UIType.Warning);
    }
}
