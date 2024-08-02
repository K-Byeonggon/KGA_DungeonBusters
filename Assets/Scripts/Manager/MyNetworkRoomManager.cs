using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNetworkRoomManager : NetworkRoomManager
{
    public static MyNetworkRoomManager Instance { get; private set; }

    [SerializeField] Character _currentCharacter;
    public Character CurrentCharacter { get { return _currentCharacter; } set { _currentCharacter = value; } }

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

        _currentCharacter = Character.Warrior;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<CharacterInfo>(OnCharacterInfoReceived);
    }

    private void OnCharacterInfoReceived(NetworkConnectionToClient conn, CharacterInfo info)
    {
        Debug.Log("CharacterInfo receiced with characterId: ");

        //RoomPlayer를 생성하기 전에 캐릭터 정보를 저장
        GameObject roomPlayer = Instantiate(roomPlayerPrefab.gameObject);
        MyNetworkRoomPlayer myRoomPlayer = roomPlayer.GetComponent<MyNetworkRoomPlayer>();
        myRoomPlayer.CurrentCharacter = info.currentCharacter;

        //RoomUI에 플레이어 띄우기(이거 서버에서만 됨)
        RoomUI roomUI = UIManager.Instance.GetCreatedUI(UIType.Room).GetComponent<RoomUI>();
        roomUI.AddPlayerPanel(info.currentCharacter);
        
        NetworkServer.AddPlayerForConnection(conn, roomPlayer);
    }



    //클라가 서버에 연결될 때, 서버에 메시지를 전송한다. (메시지: 자신의 NetworkConnection, Character)
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        
        NetworkConnection conn = NetworkClient.connection;

        // 캐릭터 정보를 서버로 전송한다. 서버에서 메시지를 받으면 OnCharacterInfoReceived 호출.
        CharacterInfo characterInfo = new CharacterInfo
        {
            currentCharacter = MyNetworkRoomManager.Instance.CurrentCharacter
        };
        conn.Send(characterInfo);
    }






    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        //추가로 캐릭터가 생성되지 않도록 비워둔다.
    }


    //이제는 안쓰이는 부분
    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
    {
        //여기서 커스텀으로 RoomPlayer 생성.
        GameObject roomPlayer = Instantiate(roomPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);
        roomPlayer.GetComponent<MyNetworkRoomPlayer>().CurrentCharacter
            = MyNetworkRoomManager.Instance.CurrentCharacter;


        return roomPlayer;
    }

    public override void OnClientError(TransportError transportError, string message)
    {
        UIManager.Instance.OpenSpecificUI(UIType.Warning);
    }
    

    public void SetCharacter(Character character)
    {
        _currentCharacter = character;
    }
}
