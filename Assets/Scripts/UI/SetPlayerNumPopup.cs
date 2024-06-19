using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerNumPopup : MonoBehaviour
{
    [SerializeField] Button Btn_FrontClose;
    [SerializeField] Button Btn_3players;
    [SerializeField] Button Btn_4players;
    [SerializeField] Button Btn_5players;

    private void OnEnable()
    {
        Btn_FrontClose.onClick.AddListener(OnClick_FrontClose);
        Btn_3players.onClick.AddListener(OnClick_3PlayerStart);
        Btn_4players.onClick.AddListener(OnClick_4PlayerStart);
        Btn_5players.onClick.AddListener(OnClick_5PlayerStart);
    }

    private void OnClick_FrontClose()
    {
        UIManager.Instance.CloseSpecificUI(UIType.SetPlayerNumPopup);
    }

    private void OnClick_3PlayerStart()
    {

    }

    private void OnClick_4PlayerStart()
    {

    }

    private void OnClick_5PlayerStart()
    {

    }
}
