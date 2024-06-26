using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class DataManagerExtension
{
    public static Monster GetMonster(this DataManager manager, int dataId)
    {
        if(manager.LoadedMonsterList.Count == 0 
            || manager.LoadedMonsterList.ContainsKey(dataId) == false)
        {
            return null;
        }
        return manager.LoadedMonsterList[dataId];
    }
}
