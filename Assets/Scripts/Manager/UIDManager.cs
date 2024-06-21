using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UIDManager : NetworkBehaviour
{
    public static UIDManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Dictionary<NetworkConnectionToClient, int> _clientUIDs = new Dictionary<NetworkConnectionToClient, int>();

    public void AddClientUID(NetworkConnectionToClient conn, int uid)
    {
        if(!_clientUIDs.ContainsKey(conn))
        {
            _clientUIDs.Add(conn, uid);
        }
    }

    public int GetClientUID(NetworkConnectionToClient conn)
    {
        if(_clientUIDs.TryGetValue(conn, out int uid))
        {
            return uid;
        }
        return -1;
    }

}
