using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNetworkRoomManager : NetworkRoomManager
{
    public static MyNetworkRoomManager Instance { get; private set; }

    public override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� ����: �� ��ȯ �� ����
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {

        base.OnServerAddPlayer(conn);
    }




    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
    {
        //���⼭ Ŀ�������� RoomPlayer ����.
        GameObject roomPlayer = Instantiate(roomPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);

        //�����Ҷ� uid������ �ȵǼ� �׳� �����Ŀ� uid �ٲ��ִ� ���� �õ��غ����� �Ѵ�.
        /*
        var myRoomPlayer = roomPlayer.GetComponent<MyNetworkRoomPlayer>();

        if (myRoomPlayer != null)
        {
            int uid = UIDManager.Instance.GetClientUID(conn);
            if (uid != -1)
            {
                myRoomPlayer.Uid = uid;
            }
            else
            {
                Debug.LogError("UID ��ã�Ҿ��");
            }
        }
        */
        return roomPlayer;
    }
}
