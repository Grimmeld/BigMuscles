using System;
using System.Runtime.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character/new")]
public class Character : ScriptableObject
{
    public string charName;
    public Texture2D image;
    
     [Range(-100,100)]
    
    public float meter;

}
