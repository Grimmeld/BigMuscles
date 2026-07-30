using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;

    [SerializeField] private Transform _pausePanel;
    [SerializeField] private Button _button;


    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(!isPaused) 
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }

        }

    }



private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        _pausePanel.gameObject.SetActive(true);
        InputManager.Instance.DisableSelect();
        InputManager.Instance.ToggleAdvance(false);

        EventSystem.current.firstSelectedGameObject = _button.gameObject;
        _button.Select();
    }


    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        _pausePanel.gameObject.SetActive(false);
        InputManager.Instance.EnableSelect();
        InputManager.Instance.ToggleAdvance(true);

    }
}
