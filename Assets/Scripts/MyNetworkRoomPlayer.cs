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
        int clientUID = LoginManager.Instance.UserID;
        UIDManager.Instance.AddClientUID(connectionToClient, clientUID);
        CmdSendUIDToServer(clientUID);
        _uid = UIDManager.Instance.GetClientUID(connectionToClient);
    }

    [Command]
    void CmdSendUIDToServer(int uid)
    {
        UIDManager.Instance.AddClientUID(connectionToClient, uid);
    }
}
