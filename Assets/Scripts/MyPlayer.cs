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
    public List<int> _cards;
    [SerializeField] List<int> _usedCards;
    [SerializeField] List<int> _jewels; //RED:0, YELLOW:1, BLUE:2
    public int _currentCard;
    public bool _isAttackSuccessed;
    public int rewardOrder;

    public List<int> Cards
    {
        get { return  _cards; }
        set { _cards = value; }
    }

    public List<int> Jewels
    {
        get { return _jewels; }
        set
        {
            _jewels = value;
        
        }
    }
    public List<int> UsedCards
    {
        get { return _usedCards; }
        set
        {
            _usedCards = value;
        
        }
    }

    private void Start()
    {
        initialSettings();

        //이 시점에 패널 생성 요청
        BattleUIManager.Instance.RequestCreatePlayer((int)this.netId);
    }

    private void initialSettings()
    {
        Cards = new List<int>() { 1,2,3,4,5,6,7 };  //3인 게임: 2~7, 4~5인 게임: 1~6
        UsedCards = new List<int>();
        Jewels = new List<int>() { 1, 1, 1 };       //RED:0, YELLOW:1, BLUE:2
    }

    public int PopCard(int num)
    {
        
        if (_cards.Contains(num))
        {
            _usedCards.Add(num);
            _cards.Remove(num);
            return num;
        }
        return -1;  //에러
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
    
    //해당 색의 보석을 전부 잃고 중앙에 쌓아둔다.
    public int LoseJewel(Jewel color)
    {
        int result = _jewels[(int)color];
        _jewels[(int)color] = 0;
        return result;
    }
}
