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

    }

    private void CreatePlayerPanel(int player_netId)
    {
        //int playerNum = MyNetworkRoomManager.Instance.minPlayers;
        //���߿��� �÷��̾� ���� ���� �� �ٸ� ������� �ٲٱ�
        var gObj = Instantiate(Prefab_PlayerSlot, Transform_SlotRoot.transform);
        var player1 = gObj.GetComponent<Panel_Player1>();
        player1.Panel_Id = player_netId;
    }
}
