using SimpleTwineDialogue;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] private TextAdventure _textAdventure;
    private InputAction _actionSelect;
    private InputAction _actionAdvance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;

        _actionSelect = gameObject.GetComponent<PlayerInput>().actions.FindActionMap("Player").FindAction("Select");
        _actionAdvance = gameObject.GetComponent<PlayerInput>().actions.FindActionMap("Player").FindAction("Advance");
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
                Debug.Log("On select choice");
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
            Debug.Log("Select button : Event system ");
            Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            ExecuteEvents.Execute<IPointerClickHandler>(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        }
    } 

    public void DisableSelect()
    {
        _actionSelect.Disable();
    }
    public void EnableSelect()
    {
        _actionSelect.Enable();
    }

    public void ToggleAdvance(bool activate)
    {
        if(activate)
        {
            _actionAdvance.Enable();
        }
        else
        {
            _actionAdvance.Disable();
        }
    }

}
