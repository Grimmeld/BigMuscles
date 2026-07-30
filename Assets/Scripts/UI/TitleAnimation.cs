using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleAnimation : MonoBehaviour
{
    [SerializeField] private CanvasGroup chapter;
    [SerializeField] private CanvasGroup subChapter;
    [SerializeField] private float outAnimationDelay;

    private void OnEnable()
    {
        chapter.transform.localPosition = new Vector2(Screen.width, -100);
        chapter.alpha = 0;
        subChapter.transform.localPosition = new Vector2(Screen.width, -200);
        subChapter.alpha = 0;


        TitleDisplay(chapter, 0f);
        TitleDisplay(subChapter, 1f);

        Invoke("OutAnimation", outAnimationDelay);
    }

    private void OutAnimation()
    {
        chapter.transform.LeanMoveLocalX(-Screen.width, 2f).setEaseOutExpo().setOnComplete(DisableScript);
        chapter.LeanAlpha(0, 0.5f);
        subChapter.transform.LeanMoveLocalX(-Screen.width, 2f).setEaseOutExpo().setOnComplete(DisableScript);
        subChapter.LeanAlpha(0, 0.5f);

    }

    private void DisableScript()
    {
        
        this.gameObject.SetActive(false);
    }

    private void TitleDisplay(CanvasGroup group, float delay)
    {
        group.transform.LeanMoveLocalX(0, 1f).setEaseOutExpo().setDelay(delay);
        group.LeanAlpha(1, 0.75f).setEaseOutCirc().setDelay(delay);
    }
}
