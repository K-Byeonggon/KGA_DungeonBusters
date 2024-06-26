using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Panel_Rewards : MonoBehaviour
{
    [SerializeField] GameObject Transform_SlotRoot;
    [SerializeField] GameObject Prefab_RewardSlot;

    public void Start()
    {
        SetRewardUI();
    }

    private void SetRewardUI()
    {

        for (int i = 1; i < 4; i++)
        {
            var gObj = Instantiate(Prefab_RewardSlot, Transform_SlotRoot.transform);
            var reward1 = gObj.GetComponent<Panel_Reward1>();
            reward1.SetRewardInfo(i);

        }

    }
}
