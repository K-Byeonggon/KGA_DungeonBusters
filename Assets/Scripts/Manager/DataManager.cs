using System.Collections;
using System.Collections.Generic;
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

    }
}
