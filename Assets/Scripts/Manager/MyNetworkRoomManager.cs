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

    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
    {
        //여기서 커스텀으로 RoomPlayer 생성.
        GameObject roomPlayer = Instantiate(roomPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);

        var myRoomPlayer = roomPlayer.GetComponent<MyNetworkRoomPlayer>();

        if (myRoomPlayer != null)
        {
            myRoomPlayer.Uid = 123345; //여기에 서버 어딘가의 Dic에 conn과 함께 저장된 uid 값 넣기
        }

        return roomPlayer;
    }
}
