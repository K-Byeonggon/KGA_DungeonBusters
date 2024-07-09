using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_WinLose : MonoBehaviour
{
    [SerializeField] Button Btn_FrontClose;
    [SerializeField] Text Text_WinLose;
    [SerializeField] GameObject Transform_SlotRoot;
    [SerializeField] GameObject Prefab_CardRed;

    public void UpdateText()
    {
        bool stageClear = NewGameManager.Instance.StageClear;

        if(stageClear) { Text_WinLose.text = "토벌 성공!"; }
        else { Text_WinLose.text = "토벌 실패.."; }
    }

    public void RemoveCards()
    {
        for (int i = 0; i < Transform_SlotRoot.transform.childCount; i++)
        {
            Destroy(Transform_SlotRoot.transform.GetChild(i).gameObject);
        }
    }

    public void SetCards()
    {
        //이것도 뭐 NewGameManager에 있는거 가져와서 만들자.
        //플레이어별로 색깔이 다르면 좋겠지만..
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
