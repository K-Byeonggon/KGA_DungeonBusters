using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    Dictionary<int, Monster> _loadedMonsterList = new Dictionary<int, Monster>();

    private readonly string _dataRootPath = "C:/Unity/DungeonBusters/DataParser";

    private void Awake()
    {
        Instance = this;
        ReadAllDataOnAwake();
    }

    private void ReadAllDataOnAwake()
    {
        ReadData(nameof(Monster));
    }

    private void ReadData(string tableName)
    {
        switch(tableName)
        {
            case "Stage1":
                ReadMonsterTable(tableName);
                break;
        }
    }

    private void ReadMonsterTable(string tableName)
    {
        XDocument doc = XDocument.Load()
            //몬스터 정보가 Monster가 아니라 Stage1에 저장되있어서 고민을 좀 해봐야
    }
}
