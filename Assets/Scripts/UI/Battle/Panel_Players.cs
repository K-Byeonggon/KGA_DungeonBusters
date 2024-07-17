using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Players : MonoBehaviour
{
    [SerializeField] GameObject Transform_SlotRoot;
    [SerializeField] GameObject Prefab_PlayerSlot;

    public void Start()
    {
        SetPlayerUI();
    }

    private void SetPlayerUI()
    {
        //int playerNum = MyNetworkRoomManager.Instance.minPlayers;
        //���߿��� �÷��̾� ���� ���� �� �ٸ� ������� �ٲٱ�
        int playerNum = 3;
        for (int i = 0; i < playerNum; i++)
        {
            var gObj = Instantiate(Prefab_PlayerSlot, Transform_SlotRoot.transform);
            var player1 = gObj.GetComponent<Panel_Player1>();
            player1.SetPlayerInfo(i);

        }
    }
}
