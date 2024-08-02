using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostumeUI : MonoBehaviour
{
    [SerializeField] Button Btn_Warrior;
    [SerializeField] Button Btn_Archer;
    [SerializeField] Button Btn_Wizard;
    [SerializeField] Button Btn_FrontClose;

    [SerializeField] LobbyPlayer lobbyPlayer;

    private void OnEnable()
    {
        Btn_Warrior.onClick.AddListener(OnClick_Warrior);
        Btn_Archer.onClick.AddListener(OnClick_Archer);
        Btn_Wizard.onClick.AddListener(OnClick_Wizard);
        Btn_FrontClose.onClick.AddListener(OnClick_FrontClose);

        lobbyPlayer = FindObjectOfType<LobbyPlayer>();
    }

    private void OnDisable()
    {
        Btn_Warrior.onClick?.RemoveListener(OnClick_Warrior);
        Btn_Archer.onClick?.RemoveListener(OnClick_Archer);
        Btn_Wizard.onClick?.RemoveListener(OnClick_Wizard);
        Btn_FrontClose.onClick?.RemoveListener(OnClick_FrontClose);
    }

    private void OnClick_Warrior()
    {
        //뭔가에 정보 넘김
        lobbyPlayer.CharacterChange(Character.Warrior);
        MyNetworkRoomManager.Instance.SetCharacter(Character.Warrior);
        UIManager.Instance.CloseSpecificUI(UIType.Costume);
    }

    private void OnClick_Archer()
    {
        //뭔가에 정보 넘김
        lobbyPlayer.CharacterChange(Character.Archer);
        MyNetworkRoomManager.Instance.SetCharacter(Character.Archer);
        UIManager.Instance.CloseSpecificUI(UIType.Costume);
    }

    private void OnClick_Wizard()
    {
        //뭔가에 정보 넘김
        lobbyPlayer.CharacterChange(Character.Wizard);
        MyNetworkRoomManager.Instance.SetCharacter(Character.Wizard);
        UIManager.Instance.CloseSpecificUI(UIType.Costume);
    }

    private void OnClick_FrontClose()
    {
        UIManager.Instance.CloseSpecificUI(UIType.Costume);
    }
}
