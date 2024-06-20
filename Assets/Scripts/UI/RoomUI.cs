using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] List<Text> Text_Players;
    [SerializeField] Button Btn_Ready;


    private void OnEnable()
    {
        Btn_Ready.onClick.AddListener(OnClick_Ready);
    }

    private void OnDisable()
    {
        
    }

    private void OnClick_Ready()
    {
        Debug.Log("¡ÿ∫Ò");
    }
}
