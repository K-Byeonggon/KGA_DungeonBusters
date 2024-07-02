using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_RemoveJewels : MonoBehaviour
{
    [Header("BonusJewels")]
    [SerializeField] Text Text_JewelRed;
    [SerializeField] Text Text_JewelYellow;
    [SerializeField] Text Text_JewelBlue;

    [Header("SelectColor")]
    [SerializeField] GameObject Transform_SlotRoot;
    [SerializeField] GameObject Prefab_Jewel;

    public void RemoveJewels()
    {
        for (int i = 0; i < Transform_SlotRoot.transform.childCount; i++)
        {
            Destroy(Transform_SlotRoot.transform.GetChild(i).gameObject);
        }
    }

    public void SetJewels()
    {
        MyPlayer player = NetworkClient.localPlayer.GetComponent<MyPlayer>();

        for(int i = 0; i < player.Jewels.Count; i++)
        {
            var gObj = Instantiate(Prefab_Jewel, Transform_SlotRoot.transform);
            var jewel_n = gObj.GetComponent<Content_Jewel>();
            jewel_n.SetColor(i);
            jewel_n.SetCount(player.Jewels[i]);
            jewel_n.SetPopupSelectUI(this);
        }

    }

    public void UISetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }
}
