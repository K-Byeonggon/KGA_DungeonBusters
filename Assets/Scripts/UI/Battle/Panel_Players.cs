using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Players : MonoBehaviour
{
    [SerializeField] GameObject Transform_SlotRoot;
    [SerializeField] GameObject Prefab_PlayerSlot;

    public void Start()
    {

    }

    //GamePlayer가 생성되고 이게 호출되어야 Panel의 Id를 설정할 수 있다?
    //아니면 그냥 GamePlayer가 생성될때 netId를 어딘가 저장해 놨다가 나중에 Panel의 Id를 설정해주는거는?
    //근데 타이밍을 언제로 잡아야되는데? GamePlayer 생성될고 나서는 타이밍이 명확한데 
    public void CreatePlayerPanel(int player_netId)
    {
        var gObj = Instantiate(Prefab_PlayerSlot, Transform_SlotRoot.transform);
        var player1 = gObj.GetComponent<Panel_Player1>();
        player1.Panel_Id = player_netId;
    }
}
