using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Player1 : MonoBehaviour
{
    [SerializeField] int _panel_Id;

    [SerializeField] Image Img_Player;
    [SerializeField] Text Text_JewelRed_Player;
    [SerializeField] Text Text_JewelYellow_Player;
    [SerializeField] Text Text_JewelBlue_Player;
    [SerializeField] Text Text_UsedCard;

    public int Panel_Id
    {
        get { return _panel_Id; } 
        set
        {
            _panel_Id = value;
        }
    }


    public void SetPlayerInfo(int playerNum)
    {
        Debug.Log($"Panel_Id:{Panel_Id}, localPlayer.netId{NetworkClient.localPlayer.netId}");

        if(Panel_Id == NetworkClient.localPlayer.netId)
        {
            Img_Player.color = Color.red;
        }

        if (NetworkClient.localPlayer != null)
        {
            MyPlayer myPlayer = NetworkClient.localPlayer.GetComponent<MyPlayer>();

            Text_JewelRed_Player.text = $"{myPlayer.Jewels[0]}";
            Text_JewelYellow_Player.text = $"{myPlayer.Jewels[1]}";
            Text_JewelBlue_Player.text = $"{myPlayer.Jewels[2]}";
        }

    }
}
