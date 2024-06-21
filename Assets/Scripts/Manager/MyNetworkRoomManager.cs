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
            DontDestroyOnLoad(gameObject); // 선택 사항: 씬 전환 시 유지
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {

        base.OnServerAddPlayer(conn);
    }




    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
    {
        //여기서 커스텀으로 RoomPlayer 생성.
        GameObject roomPlayer = Instantiate(roomPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);

        //생성할때 uid설정이 안되서 그냥 생성후에 uid 바꿔주는 것을 시도해보려고 한다.
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
                Debug.LogError("UID 못찾았어용");
            }
        }
        */
        return roomPlayer;
    }
}
