using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Content_BonusJewel : MonoBehaviour
{
    [SerializeField] Image Img_Jewel;
    [SerializeField] Button Btn_Jewel;
    [SerializeField] Text Text_Count;
    private Popup_GetBonus Popup_GetBonus;
    private int jewelIndex;

    [Header("Color 0:Red 1:Yellow 2:Blue")]
    [SerializeField] List<Sprite> Sprite_color;

    public void SetColor(int index)
    {
        Img_Jewel.sprite = Sprite_color[index];
        jewelIndex = index;
    }

    public void SetPopupGetBonus(Popup_GetBonus ui)
    {
        Popup_GetBonus = ui;
    }

    public void SetCount(int num)
    {
        Text_Count.text = $"{num}";
    }

    public void OnClick_Jewel()
    {
        //LocalPlayer의 해당 index의 보너스 Jewel +1
        // 
        
        //Popup창 꺼줌
        Popup_GetBonus.UISetActive(false);


    }
}
