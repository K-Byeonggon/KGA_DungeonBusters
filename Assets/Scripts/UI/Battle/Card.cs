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
        //NewGameManager�� ��ȣ�� localPlayer�� Netid�� �Բ� �ѱ��(�̰� ���������� �̷�������ϳ�?)
        int cardNum = int.Parse(Text_Number.text);
        NewGameManager.Instance.CmdAddSubmittedCard(NetworkClient.localPlayer, cardNum);

        //�׸��� ����â �ݱ�(�̰� ��� Ŭ�󿡼� �̷��������)
        Popup_Select.UISetActive(false);
    }


}
