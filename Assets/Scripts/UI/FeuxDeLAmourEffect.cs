using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class FeuxDeLAmourEffect : MonoBehaviour
{
    [System.Serializable]
    public class FAMGroupImage
    {
        public CanvasGroup charImage;
        public CanvasGroup actImage;
        public CanvasGroup textName;
    }

    public List<CanvasGroup> slots;
    public List<FAMGroupImage> fAMGroups;
    private int idx = 0;


    private void OnEnable()
    {
        //for (int i = 0; i < slots.Count; i++)
        //{

        //    slots[i].alpha = 0;
        //}

        for (int i = 0; i < fAMGroups.Count; i++)
        {

            fAMGroups[i].charImage.alpha = 0;
            fAMGroups[i].actImage.alpha = 0;
            fAMGroups[i].textName.alpha = 0;
        }

        InvokeRepeating("LoopFAMgroup", 3.5f, 6);

        
    }

    private void LoopFAMgroup()
    {
        Debug.Log(idx);
        fAMGroups[idx].charImage.LeanAlpha(0, 0.5f);
        fAMGroups[idx].actImage.LeanAlpha(0, 0.5f);
        fAMGroups[idx].textName.LeanAlpha(0, 0.5f);

        idx++;

        if (idx >= fAMGroups.Count)
        { idx = 0; }

        fAMGroups[idx].charImage.LeanAlpha(1, 2f).delay = 1f;
        fAMGroups[idx].actImage.LeanAlpha(1, 2f).delay = 2f;
        fAMGroups[idx].textName.LeanAlpha(1, 1f).delay = 2.2f;


    }

    //IEnumerator TypeWritingText()
    //{
    //    string textToDisplay = fAMGroups[idx].charName.text;

    //    fAMGroups[idx].charName.text = "";
    //    for (int i = 0; i < textToDisplay.Length; i++)
    //    {
    //        fAMGroups[idx].charName.text = textToDisplay.Substring(0, i);
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    private void LoopImage()
    {
        Debug.Log(idx);
        
        CancelImage(idx);

        idx++;

        if (idx >= slots.Count)
        { idx = 0; }

        DisplayImage(idx);
    
    }

    private void DisplayImage(int i)
    {
        slots[i].LeanAlpha(1, 0.75f);
    }

    private void CancelImage(int i)
    {
        slots[i].LeanAlpha(0, 0.5f).delay = 0.5f;
    }

    private void DisplayFAMImage(int i)
    {
        slots[i].LeanAlpha(1, 0.75f);
    }

    private void CancelFAMImage(int i)
    {
        slots[i].LeanAlpha(0, 0.5f).delay = 0.5f;
    }

}
