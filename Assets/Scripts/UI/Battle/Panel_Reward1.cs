using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Reward1 : MonoBehaviour
{
    [SerializeField] Text Text_RewardNum;
    [SerializeField] Text Text_JewelRed;
    [SerializeField] Text Text_JewelYellow;
    [SerializeField] Text Text_JewelBlue;

    public void SetRewardInfo(int rewardNum)
    {
        var monster = NewGameManager.Instance.CurrentMonster;
        var rewards = new List<int>();
        switch (rewardNum)
        {
            case 1:
                rewards = monster.Reward[0];
                break;
            case 2:
                rewards = monster.Reward[1];
                break;
            case 3:
                rewards = monster.Reward[2];
                break;
        }

        if (rewards == null) { gameObject.SetActive(false); return; }

        Text_RewardNum.text = $"Reward{rewardNum}";
        Text_JewelRed.text = $"{rewards[0]}";
        Text_JewelYellow.text = $"{rewards[1]}";
        Text_JewelBlue.text = $"{rewards[2]}";
        
    }
}
