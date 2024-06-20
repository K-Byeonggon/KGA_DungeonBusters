using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [Header("Panel_Players")]
    [SerializeField] Image Img_Player1;
    [SerializeField] Text Text_JewelRed_Player1;
    [SerializeField] Text Text_JewelYellow_Player1;
    [SerializeField] Text Text_JewelBlue_Player1;
    [SerializeField] Text Text_Used_Player1;

    [SerializeField] Image Img_Player2;
    [SerializeField] Text Text_JewelRed_Player2;
    [SerializeField] Text Text_JewelYellow_Player2;
    [SerializeField] Text Text_JewelBlue_Player2;
    [SerializeField] Text Text_Used_Player2;

    [SerializeField] Image Img_Player3;
    [SerializeField] Text Text_JewelRed_Player3;
    [SerializeField] Text Text_JewelYellow_Player3;
    [SerializeField] Text Text_JewelBlue_Player3;
    [SerializeField] Text Text_Used_Player3;

    [Header("Panel_Rewards")]
    [SerializeField] Text Text_JewelRed_Reward;
    [SerializeField] Text Text_JewelYellow_Reward;
    [SerializeField] Text Text_JewelBlue_Reward;

    [Header("Panel_Enemy")]
    [SerializeField] Text Text_MonsterName;
    [SerializeField] Slider Slider_Hp;
    [SerializeField] Text Text_Hp;

    [Header("Panel_Stage")]
    [SerializeField] Text Text_CurrentStage;
    [SerializeField] Text Text_CurrentRemain;

    [Header("Panel_Button")]
    [SerializeField] Button Btn_Select;

    [Header("Popup_Select")]
    [SerializeField] Button Btn_FrontClose;
    [SerializeField] Button Btn_Card1;
    [SerializeField] Button Btn_Card2;
    [SerializeField] Button Btn_Card3;
    [SerializeField] Button Btn_Card4;
    [SerializeField] Button Btn_Card5;
    [SerializeField] Button Btn_Card6;
    [SerializeField] Button Btn_Card7;

    [Header("Panel_OpenCard")]
    [SerializeField] Image Img_Card_Player1;
    [SerializeField] Image Image_Card_Player2;
    [SerializeField] Image Image_Card_Player3;
    [SerializeField] Text Text_Result;



}
