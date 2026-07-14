using SimpleTwineDialogue;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextAdventure textAdventure;
    [SerializeField] private Button nextButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
     public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (textAdventure.currentlyWriting())
        textAdventure.ShowAllPassageText();

        else
        {
            ExecuteEvents.Execute<IPointerClickHandler>(nextButton.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        }
    }
}
