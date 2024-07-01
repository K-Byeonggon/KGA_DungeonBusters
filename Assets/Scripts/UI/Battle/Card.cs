using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Unity.VisualScripting.AssemblyQualifiedNameParser;

public enum CardColor
{
    RED,
    GREEN,
    YELLOW,
    BLUE,
    PURPLE
}

public class Card : NetworkBehaviour
{
    [SerializeField] Image Img_Card;
    [SerializeField] Button Btn_Card;
    [SerializeField] Text Text_Number;
    [SerializeField] Popup_Select Popup_Select;

    [Header("Color 0:Red 1:Green 2:Yellow 3:Blue 4:Purple")]
    [SerializeField] List<Sprite> Sprite_color;

    public void SetColor(int index)
    {
        Img_Card.sprite = Sprite_color[index];
    }

    public void SetPopupSelectUI(Popup_Select ui)
    {
        Popup_Select = ui;
    }

    public void SetNumber(int num)
    {
        Text_Number.text = $"{num}";
    }


    public void OnClick_Card()
    {
        //NewGameManager에 번호를 localPlayer의 Netid와 함께 넘기기(이거 서버에서만 이루어져야하나?)
        int cardNum = int.Parse(Text_Number.text);
        NewGameManager.Instance.CmdAddSubmittedCard(NetworkClient.localPlayer, cardNum);

        //그리고 선택창 닫기(이건 모든 클라에서 이루어져야해)
        Popup_Select.UISetActive(false);
    }


}
