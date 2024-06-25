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
        Debug.Log("준비");
        //int uid = LoginManager.Instance.UserID;
        //RoomPlayers[uid].GetComponent<NetworkRoomPlayer>().CmdChangeReadyState(true);
        //지금 딕셔너리에 플레이어가 저장이 되지 않는것 같아요.
        //플레이어 uid 저장 없이 그냥 룸 입장해서 일단 게임 룰을 완성시켜 봅시다.
        //그러면 딕셔너리에 저장된 플레이어를 찾아서 준비하는게 아니라 그냥 준비 시켜야 겠네?
        //어.. 근데 그냥 룸플레이어 어떻게 찾음?

        var localPlayer = NetworkClient.connection.identity.GetComponent<MyNetworkRoomPlayer>();
        if (localPlayer != null)
        {
            localPlayer.CmdChangeReadyState(true);
        }
    }
}
