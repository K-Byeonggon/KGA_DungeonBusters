using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class DataManagerExtension
{
    public static Monster GetMonster(this DataManager manager, int dataId)
    {
        var loadedMonsterList = manager.LoadedMonsterList;
        if(manager.LoadedMonsterList.Count == 0 
            || manager.LoadedMonsterList.ContainsKey(dataId) == false)
        {
            return null;
        }
        return loadedMonsterList[dataId];
    }
}
