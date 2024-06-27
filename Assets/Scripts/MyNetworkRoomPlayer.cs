using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNetworkRoomPlayer : NetworkRoomPlayer
{
    [SyncVar][SerializeField] int _uid;
    public int Uid {  get { return _uid; }  set { _uid = value; } }


    public override void OnStartClient()
    {
        if (this.isLocalPlayer)
        {
            Debug.Log($"RoomPlayer(Local).netId = {this.netId}");
        }
        else
        {
            Debug.Log($"RoomPlayer.netId = {this.netId}");
        }
    }

    public override void OnClientEnterRoom()
    {
        //이거도 잠시 uid가져오는 시도는 포기합시다~
        /*
        int clientUID = LoginManager.Instance.UserID;
        UIDManager.Instance.AddClientUID(connectionToClient, clientUID);
        CmdSendUIDToServer(clientUID);
        _uid = UIDManager.Instance.GetClientUID(connectionToClient);
        */
    }


    //여기서 uid가져오고 있는데?

    [Command]
    void CmdSendUIDToServer(int uid)
    {
        UIDManager.Instance.AddClientUID(connectionToClient, uid);
    }
}
