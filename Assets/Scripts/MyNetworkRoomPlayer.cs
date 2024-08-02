using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNetworkRoomPlayer : NetworkRoomPlayer
{
    [SyncVar][SerializeField] int _uid;
    public int Uid {  get { return _uid; }  set { _uid = value; } }

    [SyncVar(hook = nameof(OnCharacterInfoChanged))]
    [SerializeField] Character _currentCharacter;
    public Character CurrentCharacter { get { return _currentCharacter; } set { _currentCharacter = value; } }



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



    [Command]
    private void CmdSendPlayerInfo(Character character)
    {
        this.CurrentCharacter = character;
    }

    private void OnCharacterInfoChanged(Character oldCharacter, Character newCharacter)
    {

    }


}
