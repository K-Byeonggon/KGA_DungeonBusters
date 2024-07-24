using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointUI : MonoBehaviour
{
    [SerializeField] GameObject Transform_SlotRoot;
    [SerializeField] GameObject Prefab_Point;


    private void Start()
    {
        CreatePointPanel();
    }

    //이건 안쓸거 같긴함.
    public void RemovePoints()
    {
        for (int i = 0; i < Transform_SlotRoot.transform.childCount; i++)
        {
            Destroy(Transform_SlotRoot.transform.GetChild(i).gameObject);
        }
    }

    //플레이어 수 만큼 생성.
    public void CreatePointPanel()
    {
        foreach(var player in NetworkClient.spawned)
        {
            if (player.Value.GetComponent<MyPlayer>() == null) continue;

            Debug.Log($"{player.Value.name}");
            
            var gObj = Instantiate(Prefab_Point, Transform_SlotRoot.transform);
            var point = gObj.GetComponent<Content_Point>();
            point.Panel_Id = player.Key;
        }
    }
}
