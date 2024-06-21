using Mirror;
using UnityEngine;

public class ClientUIDSender : NetworkBehaviour
{
    public int clientUID;

    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer)
        {
            clientUID = LoginManager.Instance.UserID;
            CmdSendUIDToServer(clientUID);
        }
    }

    [Command]
    void CmdSendUIDToServer(int uid)
    {
        UIDManager.Instance.AddClientUID(connectionToClient, uid);
    }

}