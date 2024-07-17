using Mirror;
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
    private int jewelIndex;

    [Header("Color 0:Red 1:Yellow 2:Blue")]
    [SerializeField] List<Sprite> Sprite_color;

    public void SetColor(int index)
    {
        Img_Jewel.sprite = Sprite_color[index];
        jewelIndex = index;
    }

    public void SetPopupRemoveJewels(Popup_RemoveJewels ui)
    {
        Popup_RemoveJewels = ui;
    }

    public void SetCount(int num)
    {
        Text_Count.text = $"{num}";
    }

    public void OnClick_Jewel()
    {
        //Ŭ���� �÷��̾��� netId�� ������ Jewel�� Index�� NewGameManager�� SelectedJewelIndexList�� ����ȴ�.
        NewGameManager.Instance.SelectedJewelIndexList.Add(NetworkClient.localPlayer.netId, jewelIndex);
        
        //Popupâ ����
        Popup_RemoveJewels.UISetActive(false);

        //���⼭ �й��÷��̾ �� üũ�ߴ��� üũ
        NewGameManager.Instance.RemoveJewelsAndSetBonus();


    }
}
