using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager
{
    private static RoomManager _instance = null;
    public static RoomManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RoomManager();
            }
            return _instance;
        }
    }

    public Dictionary<int, GameObject> RoomPlayers = new Dictionary<int, GameObject>(); 

    public void RoomUI_OnClick_Ready_CallCmd()
    {
        Debug.Log("�غ�");
        //int uid = LoginManager.Instance.UserID;
        //RoomPlayers[uid].GetComponent<NetworkRoomPlayer>().CmdChangeReadyState(true);
        //���� ��ųʸ��� �÷��̾ ������ ���� �ʴ°� ���ƿ�.
        //�÷��̾� uid ���� ���� �׳� �� �����ؼ� �ϴ� ���� ���� �ϼ����� ���ô�.
        //�׷��� ��ųʸ��� ����� �÷��̾ ã�Ƽ� �غ��ϴ°� �ƴ϶� �׳� �غ� ���Ѿ� �ڳ�?
        //��.. �ٵ� �׳� ���÷��̾� ��� ã��?

        var localPlayer = NetworkClient.connection.identity.GetComponent<MyNetworkRoomPlayer>();
        if (localPlayer != null)
        {
            localPlayer.CmdChangeReadyState(true);
        }
    }
}
