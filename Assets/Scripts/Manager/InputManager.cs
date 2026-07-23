using SimpleTwineDialogue;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] private TextAdventure _textAdventure;
    private InputAction _actionSelect;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;

        _actionSelect = gameObject.GetComponent<PlayerInput>().actions.FindActionMap("Player").FindAction("Select");
    }

    public void SetTextAdventure(TextAdventure textAdventure)
    {
        _textAdventure = textAdventure;
    }

    public void onAdvanceSequence(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_textAdventure == null)
                return;
            _textAdventure.ShowAllPassageText();

        }


    }

    public void OnSelectChoice(InputAction.CallbackContext context)
    {
            if (context.canceled)
            {
                if (_textAdventure == null)
                    return;

                if(!_textAdventure.currentlyWriting())
                {
                    SelectButton();
                }

            }
        
    }

    public void OnSelectMenu(InputAction.CallbackContext context)
    {
        if (context.canceled)
        SelectButton();

    }
    public void SelectButton()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            ExecuteEvents.Execute<IPointerClickHandler>(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        }
    }



    public void CanceledSelect()
    {
        _actionSelect.canceled -= InputManager.Instance.OnSelectChoice;
        _actionSelect.Disable();
    }
    public void EnableSelect()
    {
        _actionSelect.canceled += InputManager.Instance.OnSelectChoice;
        _actionSelect.Enable();
    }

}
