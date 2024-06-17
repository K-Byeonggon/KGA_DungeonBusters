using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SigninUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] InputField Input_Id;
    [SerializeField] InputField Input_Password;
    [SerializeField] Button Btn_Sign;
    [SerializeField] Text Text_Error;

    private void OnEnable()
    {
        Btn_Sign.onClick.AddListener(OnClick_Sign);
    }

    private void OnDisable()
    {
        Btn_Sign.onClick.RemoveListener(OnClick_Sign);
    }

    public void OnClick_Sign()
    {
        //로직을 통해 요청
    }
}
