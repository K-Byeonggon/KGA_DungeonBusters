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
        //순서에 유의. SetActive(false)가 먼저 일어나야 DB가 다시 열린다
        SignupUI.SetActive(false);
        LoginUI.SetActive(true);
    }

    public void ShowSignupUI()
    {
        LoginUI.SetActive(false);
        SignupUI.SetActive(true);
    }
}
