using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Player1 : MonoBehaviour
{
    [SerializeField] Image Img_Player;
    [SerializeField] Text Text_JewelRed_Player;
    [SerializeField] Text Text_JewelYellow_Player;
    [SerializeField] Text Text_JewelBlue_Player;
    [SerializeField] Text Text_UsedCard;


    public void SetPlayerInfo(int playerNum)
    {
        if(NetworkClient.localPlayer != null)
        {
            MyPlayer myPlayer = NetworkClient.localPlayer.GetComponent<MyPlayer>();

            Text_JewelRed_Player.text = $"{myPlayer.Jewels[0]}";
            Text_JewelYellow_Player.text = $"{myPlayer.Jewels[1]}";
            Text_JewelBlue_Player.text = $"{myPlayer.Jewels[2]}";
        }

    }
}
