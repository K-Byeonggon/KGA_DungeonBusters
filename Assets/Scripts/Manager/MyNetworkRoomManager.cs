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

    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("안녕하세요");
        base.OnServerAddPlayer(conn);
        Debug.Log("반갑습니다.");
    }



    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
    {
        //여기서 커스텀으로 RoomPlayer 생성.
        GameObject roomPlayer = Instantiate(roomPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);
        roomPlayer.GetComponent<MyNetworkRoomPlayer>().CurrentCharacter = MyNetworkRoomManager.Instance.CurrentCharacter;


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
