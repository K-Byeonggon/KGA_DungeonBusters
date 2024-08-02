using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    [SerializeField] GameObject Warrior;
    [SerializeField] GameObject Archer;
    [SerializeField] GameObject Wizard;

    private Animator _animator;

    public void CharacterChange(Character character)
    {
        switch (character)
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
        _animator.SetTrigger("Select");
    }
}
