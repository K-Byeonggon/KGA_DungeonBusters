using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject LoginUI;
    [SerializeField] GameObject SignupUI;

    private static UIManager _instance = null;

    public static UIManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void ShowLoginUI()
    {
        LoginUI.SetActive(true);
        SignupUI.SetActive(false);
    }

    public void ShowSignupUI()
    {
        SignupUI.SetActive(true);
        LoginUI.SetActive(false);
    }
}
