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
        //LocalPlayer�� ������ �ִ� ī�� ��� �޾Ƽ�,
        MyPlayer player = NetworkClient.localPlayer.GetComponent<MyPlayer>();

        //Prefab�� ���ڵ� ä���� �����ϸ� �ɵ�.
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
        //���� ���� �ݱ�
        UISetActive(false);
    }
}
