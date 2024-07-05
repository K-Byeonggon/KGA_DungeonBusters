using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_GetBonus : MonoBehaviour
{
    [Header("SelectColor")]
    [SerializeField] GameObject Panel_SelectColor;
    [SerializeField] GameObject Transform_SlotRoot;
    [SerializeField] GameObject Prefab_Jewel;

    [Header("WaitForSelect")]
    [SerializeField] GameObject Panel_WaitForSelect;

    public void RemoveJewels()
    {
        for (int i = 0; i < Transform_SlotRoot.transform.childCount; i++)
        {
            Destroy(Transform_SlotRoot.transform.GetChild(i).gameObject);
        }
    }

    public void SetJewels()
    {
        List<int> bonus = NewGameManager.Instance.BonusJewels;
        for (int i = 0; i < bonus.Count; i++)
        {
            if (bonus[i] == 0) continue;
            var gObj = Instantiate(Prefab_Jewel, Transform_SlotRoot.transform);
            var jewel_n = gObj.GetComponent<Content_BonusJewel>();
            jewel_n.SetColor(i);
            jewel_n.SetCount(bonus[i]);
            jewel_n.SetPopupGetBonus(this);
        }
    }

    public void UISetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }

    public void UISetPanelType(bool getBonus)
    {
        Panel_SelectColor.SetActive(getBonus);
        Panel_WaitForSelect.SetActive(!getBonus);
    }
}
