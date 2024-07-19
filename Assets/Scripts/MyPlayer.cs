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

    public Animator _animator;


    public List<int> Cards
    {
        get { return  _cards; }
        set
        {
            _cards = value;
            OnPlayerCardsChanged();
        }
    }

    public List<int> Jewels
    {
        get { return _jewels; }
        set
        {
            _jewels = value;
            BattleUIManager.Instance.RequestUpdatePlayerJewels((int)this.netId);
        }
    }
    public List<int> UsedCards
    {
        get { return _usedCards; }
        set
        {
            _usedCards = value;
            BattleUIManager.Instance.RequestUpdatePlayerUsedCards((int)this.netId);
        }
    }


    public override void OnStartClient()
    {
        base.OnStartClient();

        RegisterPlayer();

        //이 시점에 패널 생성 요청
        BattleUIManager.Instance.RequestCreatePlayer((int)this.netId);

        initialSettings();

        //이 시점에 카드 선택창 생성 요청?
        BattleUIManager.Instance.RequestUpdateSelectCard();
    }

    private void RegisterPlayer()
    {
        var newList = new Dictionary<uint, MyPlayer> (NewGameManager.Instance.PlayerList);
        newList.Add(this.netId, this);
        NewGameManager.Instance.PlayerList = newList;
    }



    private void OnPlayerCardsChanged()
    {
        if (this.isLocalPlayer == false) return;
        BattleUIManager.Instance.RequestUpdateSelectCard();
    }






    private void initialSettings()
    {
        int players = MyNetworkRoomManager.Instance.minPlayers;
        if(players < 4) { Cards = new List<int>() { 2,3,4,5,6,7 }; }
        else { Cards = new List<int>() { 1, 2, 3, 4, 5, 6 }; }
        
        UsedCards = new List<int>();
        Jewels = new List<int>() { 1, 1, 1 };       //RED:0, YELLOW:1, BLUE:2
    }


    public void SetAnimator()
    {
        _animator.SetBool("Run", true);

    }

}
