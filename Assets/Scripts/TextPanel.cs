using SimpleTwineDialogue;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextPanel : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextAdventure textAdventure;
    [SerializeField] private Button nextButton;

    public Texture2D customCursor;

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

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Cursor.SetCursor(CursorManager.Instance.GetOnHoverCursor(), Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Cursor.SetCursor( null, Vector2.zero, CursorMode.Auto);

    }
}
