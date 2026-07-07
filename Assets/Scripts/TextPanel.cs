using SimpleTwineDialogue;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextAdventure textAdventure;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
     public void OnPointerClick(PointerEventData pointerEventData)
    {
        textAdventure.ShowAllPassageText();
    }
}
