using System;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class Character
{
    public string keyName;
    public string charName;
    public Texture2D image;
    
    [Range(-100,100)]
    
    public float meter;


}
