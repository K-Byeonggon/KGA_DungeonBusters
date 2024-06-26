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
            case "Monster":
                ReadMonsterTable(tableName);
                break;
        }
    }

    private void ReadMonsterTable(string tableName)
    {
        XDocument doc = XDocument.Load($"{_dataRootPath}/{tableName}.xml");
        var dataElements = doc.Descendants("data");

        foreach (var data in dataElements)
        {
            var tempMonster = new Monster();
            tempMonster.DataId = int.Parse(data.Attribute(nameof(tempMonster.DataId)).Value);
            tempMonster.Name = data.Attribute(nameof(Monster.Name)).Value;
            tempMonster.Stage = int.Parse(data.Attribute(nameof(Monster.Stage)).Value);
            tempMonster.HP = int.Parse(data.Attribute(nameof(Monster.HP)).Value);
            tempMonster.Reward1 = ReadMonsterRewards(data, nameof(tempMonster.Reward1));
            tempMonster.Reward2 = ReadMonsterRewards(data, nameof(tempMonster.Reward2));
            tempMonster.Reward2 = ReadMonsterRewards(data, nameof(tempMonster.Reward3));
            _loadedMonsterList.Add(tempMonster.DataId, tempMonster);
        }
    }

    private List<int> ReadMonsterRewards(XElement data, string Rewardn)
    {
        string rewardn_ListStr = data.Attribute(Rewardn).Value;
        if (string.IsNullOrEmpty(rewardn_ListStr))
        {
            rewardn_ListStr = rewardn_ListStr.Replace("{", string.Empty);
            rewardn_ListStr = rewardn_ListStr.Replace("}", string.Empty);

            var rewards = rewardn_ListStr.Split(',');

            if (rewards.Length > 0)
            {
                var list = new List<int>();
                foreach (var reward in rewards)
                {
                    list.Add(int.Parse(reward));
                }
                return list;
            }
            else { return null; }
        }
        else { return null; }
    }
}