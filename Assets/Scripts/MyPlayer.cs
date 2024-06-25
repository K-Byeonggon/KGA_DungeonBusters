using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Jewel
{
    RED,
    YEllOW,
    BLUE
}

public class MyPlayer : NetworkBehaviour
{
    [SerializeField] List<int> _cards = new List<int>() { 1,2,3,4,5,6,7};
    [SerializeField] List<int> _jewels = new List<int>() { 1, 1, 1 }; //RED:0, YELLOW:1, BLUE:2

    public int PopCard(int num)
    {
        if (_cards.Contains(num))
        {
            _cards.Remove(num);
            return num;
        }
        return -1;  //¿¡·¯
    }

    public void InitCards()
    {
        _cards = new List<int>() { 1,2,3,4,5,6,7 };
    }

    public void GetJewel(Jewel color, int num)
    {
        _jewels[(int)color] += num;
    }
    
    //ÇØ´ç »öÀÇ º¸¼®À» ÀüºÎ ÀÒ°í Áß¾Ó¿¡ ½×¾ÆµÐ´Ù.
    public int LoseJewel(Jewel color)
    {
        int result = _jewels[(int)color];
        _jewels[(int)color] = 0;
        return result;
    }
}
