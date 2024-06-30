using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] Button Btn_Card;
    [SerializeField] Text Text_Number;


    public void SetNumber(int num)
    {
        Text_Number.text = $"{num}";
    }


    public void OnClick_Card()
    {

    }
}
