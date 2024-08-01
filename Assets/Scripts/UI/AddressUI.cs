using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddressUI : MonoBehaviour
{
    [SerializeField] InputField Input_Address;
    [SerializeField] Button Btn_Start;
    [SerializeField] Button Btn_FrontClose;

    private void Start()
    {
        Btn_Start.onClick.AddListener(OnClick_Btn_Start);
        Btn_FrontClose.onClick.AddListener(OnClick_Btn_FrontClose);
    }


    private void OnClick_Btn_Start()
    {
        string address = Input_Address.text;

        //네트워크 매니저에 전달
        MyNetworkRoomManager.Instance.networkAddress = address;

        //클라로 게임 시작
        try
        {
            MyNetworkRoomManager.Instance.StartClient();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to start client: {e.Message}");
        }

    }

    private void OnClick_Btn_FrontClose()
    {
        UIManager.Instance.CloseSpecificUI(UIType.Address);
    }

}
