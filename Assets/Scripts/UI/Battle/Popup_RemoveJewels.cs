using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Popup_RemoveJewels : MonoBehaviour
{
    [SerializeField] GameObject Panel_Wait;
    [SerializeField] GameObject Panel_Remove;
 
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

        List<int> maxJewels = FindMaxIndexes(player.Jewels);

        foreach(int indexJewel in maxJewels)
        {
            var gObj = Instantiate(Prefab_Jewel, Transform_SlotRoot.transform);
            var jewel_n = gObj.GetComponent<Content_Jewel>();
            jewel_n.SetColor(indexJewel);
            jewel_n.SetCount(player.Jewels[indexJewel]);
            jewel_n.SetPopupRemoveJewels(this);
        }
    }

    private List<int> FindMaxIndexes(List<int> list)
    {
        int maxValue = list.Max();
        List<int> maxIndexes = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == maxValue)
            {
                maxIndexes.Add(i);
            }
        }
        return maxIndexes;
    }

    public void SetBonus()
    {
        List<int> bonus = NewGameManager.Instance.BonusJewels;
        Text_JewelRed.text = $"{bonus[0]}";
        Text_JewelYellow.text = $"{bonus[1]}";
        Text_JewelBlue.text = $"{bonus[2]}";
    }

    public void SetLosePlayer(bool isLosePlayer)
    {
        Panel_Remove.SetActive(isLosePlayer);
        Panel_Wait.SetActive(!isLosePlayer);
    }

    public void WaitForOthers()
    {
        Panel_Remove.SetActive(false);
        Panel_Wait.SetActive(true);
    }

    public void UISetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }
}
