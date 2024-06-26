using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    public int DataId { get; set; }
    public string Name { get; set; }
    public int Stage {  get; set; }
    public int HP { get; set; }
    public List<int> Reward1 {  get; set; }
    public List<int> Reward2 { get; set; }
    public List<int> Reward3 { get; set; }
}
