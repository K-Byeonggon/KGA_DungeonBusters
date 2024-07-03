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

    private void Start()
    {
        SetPlayerInfo();
    }

    public void SetPlayerInfo()
    {
        if(Panel_Id == NetworkClient.localPlayer.netId)
        {
            Img_Player.color = Color.red;
            //이부분은 나중에 프로필을 띄우는 걸로 변경
        }
    }

    public void UpdateJewelsInfo()
    {
            MyPlayer player = NetworkClient.localPlayer.GetComponent<MyPlayer>();
            Text_JewelRed_Player.text = $"{player.Jewels[0]}";
            Text_JewelYellow_Player.text = $"{player.Jewels[1]}";
            Text_JewelBlue_Player.text = $"{player.Jewels[2]}";
    }

    public void UpdateUsedCardsInfo()
    {
            MyPlayer player = NetworkClient.localPlayer.GetComponent<MyPlayer>();

            string cards = string.Empty;

            foreach(int card in player.UsedCards)
            {
                cards += $"{card} ";
            }

            Text_UsedCard.text = "Used: " + cards;

    }
}
