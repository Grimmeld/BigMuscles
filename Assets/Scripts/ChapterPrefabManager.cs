using SimpleTwineDialogue;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class ChapterPrefabManager : MonoBehaviour
{ /// <summary>
  /// Set up this parameter in the main GameObject of the Prefab 
  /// For easier modification
  /// 
  /// Put every parameter that will be different in the prefab
  /// 
  /// In this case, I don't have to double clic on the prefab, search of the textAdventure script etc etc
  /// </summary>
  /// <returns></returns>
  /// 
    [SerializeField] private string localFileName;
    [Header("Title Modifier")]
    [SerializeField] private bool hasTitleDisplayed;
    [Tooltip("The delay text appears after title")][SerializeField] private float textDelay = 0f;
    
   
    public string SetVariantFilename()
    {
        return localFileName;
    }
    public bool SetVariantTitleDisplay()
    {
        return hasTitleDisplayed;
    }

    public float SetVariantTextDelay()
    {
        return textDelay;
    }
}
