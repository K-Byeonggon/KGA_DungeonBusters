using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    public int DataId { get; set; }
    public string Name { get; set; }
    public int Dungeon { get; set; }
    public int HP { get; set; }
    public List<List<int>> Reward { get; set; } //{{red, yellow, blue}, {red, yellow, blue}, {red, yellow, blue}}

    public Monster()
    {
        Reward = new List<List<int>>();
    }

}
