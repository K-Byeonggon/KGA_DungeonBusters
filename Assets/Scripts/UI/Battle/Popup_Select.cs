using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Select : MonoBehaviour
{
    [SerializeField] Button Btn_FrontClose;
    [SerializeField] GameObject Transform_SlotRoot;
    [SerializeField] GameObject Prefab_CardRed;

    public void RemoveCards()
    {
        for (int i = 0; i < Transform_SlotRoot.transform.childCount; i++)
        {
            Destroy(Transform_SlotRoot.transform.GetChild(i).gameObject);
        }
    }

    public void SetCards()
    {
        //LocalPlayer가 가지고 있는 카드 목록 받아서,
        MyPlayer player = NetworkClient.localPlayer.GetComponent<MyPlayer>();

        //Prefab에 숫자들 채워서 생성하면 될듯.
        foreach (int card in player.Cards)
        {
            var gObj = Instantiate(Prefab_CardRed, Transform_SlotRoot.transform);
            var card_n = gObj.GetComponent<Card>();
            card_n.SetNumber(card);
            card_n.SetPopupSelectUI(this);
        }

    }

    public void UISetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }


    public void OnClick_FrontClose()
    {
        //대충 전면 닫기
        UISetActive(false);
    }
}
