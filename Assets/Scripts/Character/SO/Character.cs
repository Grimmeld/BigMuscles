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
    public int characterWidth = 500;
    public int characterHeight = 500;

    public Texture2D characterIcon;
    public Color text_color;

    public EyeImage[] eyeimages;

    [Tooltip("Affection level ; start with 0 for new game")]
    [Range(-100,100)]
    public float meter;



}

[System.Serializable]

public class EyeImage
{
    public string eyeName;
    public Texture2D eyeImage;
    public int eyeWidth = 300;
    public int eyeHeight = 300;
    public float posX = 200;
    public float posY = 0;
}
