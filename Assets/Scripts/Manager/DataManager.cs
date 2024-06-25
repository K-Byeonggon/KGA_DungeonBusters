using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    Dictionary<int, Monster> _loadedMonsterList = new Dictionary<int, Monster>();

    private readonly string _dataRootPath = "C:/Unity/DungeonBusters/DataParser";
}
