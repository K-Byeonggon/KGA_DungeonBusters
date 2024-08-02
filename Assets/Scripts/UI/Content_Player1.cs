using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Content_Player1 : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Image Img_Player;
    [SerializeField] Text Text_PlayerName;
    [SerializeField] Image Img_Color;

    [Header("Sprite등록")]
    [SerializeField] List<Sprite> Icons;

    public void SetPlayerInfo(Character character)
    {
        Text_PlayerName.text = "플레이어";
        Img_Player.sprite = Icons[(int)character];
    }
}
