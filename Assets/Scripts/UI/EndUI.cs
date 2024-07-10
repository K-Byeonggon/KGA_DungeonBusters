using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    [SerializeField] Text Text_WinLose;
    [SerializeField] Text Text_NickName;
    [SerializeField] Image Img_Player;
    [SerializeField] Text Text_PlayerName;
    [SerializeField] Button Btn_Check;


    public void UpdateTextWinLose(bool isWin)
    {
        if (isWin)
        {
            Text_WinLose.text = "우승!";
            Text_NickName.text = "가장 비열한 용사";
        }
        else
        {
            Text_WinLose.text = "아쉽네요";
            Text_NickName.text = "인간성을 버리지 못했다";
        }
    }

    public void UpdatePlayerInfo()
    {
        //로컬 플레이어의 정보를 받아야함.
    }

    public void OnClick_Check()
    {
        //로비로 보내야함.
    }

}
