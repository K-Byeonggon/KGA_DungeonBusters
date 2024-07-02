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
        //클릭시 플레이어의 netId와 선택한 Jewel의 Index가 NewGameManager의 SelectedJewelIndexList에 저장된다.
        NewGameManager.Instance.SelectedJewelIndexList.Add(NetworkClient.localPlayer.netId, jewelIndex);
        
        //Popup창 꺼줌
        Popup_RemoveJewels.UISetActive(false);

        //여기서 패배플레이어가 다 체크했는지 체크
        NewGameManager.Instance.RemoveJewelsAndSetBonus();


    }
}
