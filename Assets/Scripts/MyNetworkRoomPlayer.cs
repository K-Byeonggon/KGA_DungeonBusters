using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNetworkRoomPlayer : NetworkRoomPlayer
{
    [SerializeField] int _uid;

    private void OnEnable()
    {
        OnEnable_RegisterRoomPlayer();
    }

    private void OnEnable_RegisterRoomPlayer()
    {
        _uid = LoginManager.Instance.UserID;
        RoomManager.Instance.RoomPlayers.Add(_uid, this.gameObject);
    }

    //내장된 CmdChangeReadyState를 활용해서 준비.
}
