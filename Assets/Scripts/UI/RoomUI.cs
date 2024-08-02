using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    [SerializeField] GameObject Transform_SlotRoot;
    [SerializeField] GameObject Prefab_Player1;

    [Header("UI")]
    [SerializeField] List<Text> Text_Players;
    [SerializeField] Button Btn_Ready;


    private void OnEnable()
    {
        Btn_Ready.onClick.AddListener(OnClick_Ready);
    }

    private void OnDisable()
    {
        Btn_Ready.onClick.RemoveListener(OnClick_Ready);
    }

    private void OnClick_Ready()
    {
        RoomManager.Instance.RoomUI_OnClick_Ready_CallCmd();
    }

    public void AddPlayerPanel(Character character)
    {
        var gObj = Instantiate(Prefab_Player1, Transform_SlotRoot.transform);
        var player = gObj.GetComponent<Content_Player1>();
        player.SetPlayerInfo(character);
    }
}
