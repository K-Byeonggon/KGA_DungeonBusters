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
    public List<int> _cards = new List<int>() { 1,2,3,4,5,6,7};
    [SerializeField] List<int> _usedCards = new List<int>();
    [SerializeField] List<int> _jewels = new List<int>() { 1, 1, 1 }; //RED:0, YELLOW:1, BLUE:2
    public int _currentCard;
    public bool _isAttacked;

    public List<int> Jewels
    {
        get { return _jewels; }
        set { _jewels = value; }
    }
    public List<int> UsedCards
    {
        get { return _usedCards; }
        set { _usedCards = value; }
    }


    public int PopCard(int num)
    {
        
        if (_cards.Contains(num))
        {
            _usedCards.Add(num);
            _cards.Remove(num);
            return num;
        }
        return -1;  //¿¡·¯
    }

    public void InitCards()
    {
        _cards = new List<int>() { 1,2,3,4,5,6,7 };
        _usedCards = new List<int>();
    }

    public void AddJewel(Jewel color, int num)
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
