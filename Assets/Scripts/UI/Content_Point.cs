using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Content_Point : MonoBehaviour
{
    [SerializeField] Text Text_Player;
    [SerializeField] Image Img_Player;
    [SerializeField] Text Text_PointRed;
    [SerializeField] Text Text_PointYellow;
    [SerializeField] Text Text_PointBlue;
    [SerializeField] Text Text_PointSet;
    [SerializeField] Text Text_PointTotal;
    [SerializeField] uint _panel_Id;

    public uint Panel_Id
    {
        get { return _panel_Id; }
        set 
        {
            _panel_Id = value;
            SetPlayerInfo();
            SetPointInfo(Panel_Id);
        }
    }

    public void SetPlayerInfo()
    {
        Text_Player.text = "플레이어";
        if(Panel_Id == NetworkClient.localPlayer.netId)
        {
            Img_Player.color = Color.red;
        }
    }

    public void SetPointInfo(uint playerNetId)
    {
        MyPlayer player = NewGameManager.Instance.GetPlayerFromNetId(playerNetId);
        int pointRed = player.Jewels[0];
        int pointYellow = player.Jewels[1];
        int pointBlue = player.Jewels[2];
        int pointSet = player.Jewels.Min()*3;
        int pointTotal = pointRed+pointYellow+pointBlue+pointSet;

        Text_PointRed.text = $"{pointRed}";
        Text_PointYellow.text = $"{pointYellow}";
        Text_PointBlue.text = $"{pointBlue}";
        Text_PointSet.text = $"{pointSet}";
        Text_PointTotal.text = $"{pointTotal}";
    }

}
