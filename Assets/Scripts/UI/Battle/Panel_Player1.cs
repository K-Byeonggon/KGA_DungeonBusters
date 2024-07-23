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

    public void UpdateJewelsInfo(uint player_netId)
    {
        MyPlayer player = NewGameManager.Instance.GetPlayerFromNetId(player_netId);
        Text_JewelRed_Player.text = $"{player.Jewels[0]}";
        Text_JewelYellow_Player.text = $"{player.Jewels[1]}";
        Text_JewelBlue_Player.text = $"{player.Jewels[2]}";
    }

    public void UpdateUsedCardsInfo(uint player_netId)
    {
        //여기가 LocalPlayer만 부르니까 동일하게 된거 같은데? 같은 Player의 UsedCard만 참고 하게 되자너
        //MyPlayer player = NetworkClient.localPlayer.GetComponent<MyPlayer>();

        MyPlayer player = NewGameManager.Instance.GetPlayerFromNetId(player_netId);


        string cards = string.Empty;

        foreach(int card in player.UsedCards)
        {
            cards += $"{card} ";
        }

        Text_UsedCard.text = "Used: " + cards;

    }
}
