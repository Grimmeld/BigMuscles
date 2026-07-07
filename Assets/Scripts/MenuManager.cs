using SimpleTwineDialogue;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    private bool isPaused;

    [SerializeField] private Transform _pausePanel;

    [SerializeField] private TextAdventure textAdventure;


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
        textAdventure.CanceledSelect();
    }


    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        _pausePanel.gameObject.SetActive(false);
        textAdventure.EnableSelect();
    }
}
