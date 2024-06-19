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
        Btn_StartWithServer.onClick.AddListener(OnClick_StartWithServer);
        Btn_StartWithClient.onClick.AddListener(() => MyNetworkRoomManager.Instance.StartClient());
    }

    private void OnClick_StartWithServer()
    {
        UIManager.Instance.OpenSpecificUI(UIType.SetPlayerNumPopup);
    }


}
