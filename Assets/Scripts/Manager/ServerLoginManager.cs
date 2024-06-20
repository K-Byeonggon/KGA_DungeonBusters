using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UserIDMessage : NetworkMessage
{
    public int UserID;
    //public int currentCostume;
}


public class ServerLoginManager : NetworkBehaviour
{
    public static ServerLoginManager Instance { get; private set; }

    private Dictionary<NetworkConnection, int> _userIDs = new Dictionary<NetworkConnection, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler<UserIDMessage>(OnUserIDMessageRecived);
    }

    private void OnUserIDMessageRecived(NetworkConnection conn, UserIDMessage msg)
    {
        if(_userIDs.ContainsKey(conn) == false)
        {
            _userIDs.Add(conn, msg.UserID);
        }
    }

    
    [Server]
    public void RegisterUserID(NetworkConnection conn, int uid)
    {
        if (!_userIDs.ContainsKey(conn))
        {
            _userIDs.Add(conn, uid);
        }
    }

    [Server]
    public int GetUserID(NetworkConnection conn)
    {
        if (_userIDs.ContainsKey(conn))
        {
            return _userIDs[conn];
        }
        return -1;
    }
}
