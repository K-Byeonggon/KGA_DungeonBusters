using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Content_Card : MonoBehaviour
{
    [SerializeField] Image Img_Card;
    [SerializeField] Text Text_Number;

    [Header("Color 0:Red 1:Green 2:Yellow 3:Blue 4:Purple")]
    [SerializeField] List<Sprite> Sprite_color;

    public void SetColor(int index)
    {
        Img_Card.sprite = Sprite_color[index];
    }

    public void SetNumber(int num)
    {
        Text_Number.text = $"{num}";
    }
}
