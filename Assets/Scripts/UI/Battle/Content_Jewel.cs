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
        //Ŭ���ϸ� NewGameManager�� ���� �÷��̾ ���� �ش� ���� ������ ���� ���ʽ��� �Ű�����.
    }
}
