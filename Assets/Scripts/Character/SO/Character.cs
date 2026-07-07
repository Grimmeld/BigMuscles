using System;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Character
{
    public string keyName;
    [Tooltip("Name that will be displayed in HUD")]
    public string charName;

    public Texture2D characterImage;
    public int characterWidth;
    public int characterHeight;

    [Tooltip("Affection level ; start with 0 for new game")]
    [Range(-100,100)]
    public float meter;

    public Color text_color;


}
