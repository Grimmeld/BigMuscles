using SimpleTwineDialogue;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextPanel : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static TextPanel Instance;

    [SerializeField] private TextAdventure textAdventure;
    [SerializeField] private Button nextButton;

    public Texture2D customCursor;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (textAdventure.currentlyWriting())
        textAdventure.ShowAllPassageText();

        else
        {
            if (nextButton != null)
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

    public void SetNextButton(Button button)
    {
        nextButton = button;
    }
}
