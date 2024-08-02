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

public enum PlayerAnim
{
    Idle,
    Run,
    Atk,
    Damage,
    Lose,
    Win
}


public class MyPlayer : NetworkBehaviour
{
    public List<int> _cards;
    [SerializeField] List<int> _usedCards;
    [SerializeField] List<int> _jewels; //RED:0, YELLOW:1, BLUE:2


    [Header("Character")]
    [SerializeField] GameObject Warrior;
    [SerializeField] GameObject Archer;
    [SerializeField] GameObject Wizard;

    public Animator _animator;

    private bool _isAttackSuccessed;
    private int _submittedCard;

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
            BattleUIManager.Instance.RequestUpdatePlayerJewels(this.netId);
        }
    }
    public List<int> UsedCards
    {
        get { return _usedCards; }
        set
        {
            _usedCards = value;
            BattleUIManager.Instance.RequestUpdatePlayerUsedCards(this.netId);
        }
    }

    public bool IsAttackSuccessed
    {
        get { return _isAttackSuccessed; }
        set { _isAttackSuccessed = value; }
    }

    public int SubmittedCard
    {
        get { return _submittedCard; }
        set { _submittedCard = value; }
    }

    private void Awake()
    {
        Debug.Log("나 언제?");
        CharacterChange(Character.Archer);
    }


    private void CharacterChange(Character character)
    {
        switch(character)
        {
            case Character.Warrior:
                Warrior.SetActive(true);
                Archer.SetActive(false);
                Wizard.SetActive(false);
                _animator = Warrior.transform.GetChild(0).GetComponent<Animator>();
                break;
            case Character.Archer:
                Warrior.SetActive(false);
                Archer.SetActive(true);
                Wizard.SetActive(false);
                _animator = Archer.transform.GetChild(0).GetComponent<Animator>();
                break;
            case Character.Wizard:
                Warrior.SetActive(false);
                Archer.SetActive(false);
                Wizard.SetActive(true);
                _animator = Wizard.transform.GetChild(0).GetComponent<Animator>();
                break;
        }
    }


    public override void OnStartClient()
    {
        base.OnStartClient();

        RegisterPlayer();

        //이 시점에 패널 생성 요청
        BattleUIManager.Instance.RequestCreatePlayer(this.netId);

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


    public void SetAnimator(PlayerAnim anim)
    {
        switch (anim)
        {
            case PlayerAnim.Idle:
                _animator.SetBool("Run", false);
                _animator.SetBool("Lose", false);
                break;
            case PlayerAnim.Run:
                _animator.SetBool("Run", true);
                break;
            case PlayerAnim.Atk:
                _animator.SetTrigger("Atk");
                break;
            case PlayerAnim.Damage:
                _animator.SetTrigger("Damage");
                break;
            case PlayerAnim.Lose:
                _animator.SetBool("Lose", true);
                break;
            case PlayerAnim.Win:
                _animator.SetTrigger("Win");
                break;
        }

    }

}
