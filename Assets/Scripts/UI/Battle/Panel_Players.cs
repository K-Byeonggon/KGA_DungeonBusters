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

    //GamePlayer�� �����ǰ� �̰� ȣ��Ǿ�� Panel�� Id�� ������ �� �ִ�?
    //�ƴϸ� �׳� GamePlayer�� �����ɶ� netId�� ��� ������ ���ٰ� ���߿� Panel�� Id�� �������ִ°Ŵ�?
    //�ٵ� Ÿ�̹��� ������ ��ƾߵǴµ�? GamePlayer �����ɰ� ������ Ÿ�̹��� ��Ȯ�ѵ� 
    public void CreatePlayerPanel(int player_netId)
    {
        var gObj = Instantiate(Prefab_PlayerSlot, Transform_SlotRoot.transform);
        var player1 = gObj.GetComponent<Panel_Player1>();
        player1.Panel_Id = player_netId;
    }
}
