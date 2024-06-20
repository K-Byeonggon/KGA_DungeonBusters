using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [Header("Panel_PlayerInfo")]
    [SerializeField] Text Crown1;
    [SerializeField] Text Crown2;
    [SerializeField] Text Crown3;
    [SerializeField] Text Gold;
    [SerializeField] Button Btn_Logout;

    [Header("Panel_Buttons")]
    [SerializeField] Button Btn_StartWithServer;
    [SerializeField] Button Btn_StartWithClient;
    [SerializeField] Button Btn_Costume;



    private void OnEnable()
    {
        Btn_Logout.onClick.AddListener(OnClick_Logout);
        Btn_StartWithServer.onClick.AddListener(OnClick_StartWithServer);
        Btn_StartWithClient.onClick.AddListener(OnClick_StartWithClient);
    }

    private void OnDisable()
    {
        Btn_Logout.onClick.RemoveListener(OnClick_Logout);
        Btn_StartWithServer.onClick.RemoveListener(OnClick_StartWithServer);
        Btn_StartWithClient.onClick.RemoveListener(OnClick_StartWithClient);
    }

    private void OnClick_Logout()
    {
        UIManager.Instance.CloseSpecificUI(UIType.Lobby);
        UIManager.Instance.OpenSpecificUI(UIType.Login);
    }

    private void OnClick_StartWithServer()
    {
        UIManager.Instance.OpenSpecificUI(UIType.SetPlayerNumPopup);
    }

    private void OnClick_StartWithClient()
    {
        LobbyManager.Instance.LobbyUI_OnClick_StartWitClient();
    }
    
}
