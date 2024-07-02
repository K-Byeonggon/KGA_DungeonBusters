using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Content_Jewel : MonoBehaviour
{
    [SerializeField] Image Img_Jewel;
    [SerializeField] Button Btn_Jewel;
    [SerializeField] Text Text_Count;
    private Popup_RemoveJewels Popup_RemoveJewels;

    [Header("Color 0:Red 1:Yellow 2:Blue")]
    [SerializeField] List<Sprite> Sprite_color;

    public void SetColor(int index)
    {
        Img_Jewel.sprite = Sprite_color[index];
    }

    public void SetPopupSelectUI(Popup_RemoveJewels ui)
    {
        Popup_RemoveJewels = ui;
    }

    public void SetCount(int num)
    {
        Text_Count.text = $"{num}";
    }

    public void OnClick_Jewel()
    {
        //클릭하면 NewGameManager에 의해 플레이어가 가진 해당 색의 보석이 전부 보너스로 옮겨진다.
    }
}
