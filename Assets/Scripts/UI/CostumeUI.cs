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

    private void OnEnable()
    {
        Btn_Warrior.onClick.AddListener(OnClick_Warrior);
        Btn_Archer.onClick.AddListener(OnClick_Archer);
        Btn_Wizard.onClick.AddListener(OnClick_Wizard);
    }

    private void OnDisable()
    {
        Btn_Warrior.onClick?.RemoveListener(OnClick_Warrior);
        Btn_Archer.onClick?.RemoveListener(OnClick_Archer);
        Btn_Wizard.onClick?.RemoveListener(OnClick_Wizard);
    }

    private void OnClick_Warrior()
    {

    }

    private void OnClick_Archer()
    {

    }

    private void OnClick_Wizard()
    {

    }

    private void OnClick_FrontClose()
    {

    }
}
