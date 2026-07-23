using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<Transform> canvas;

    public string buttonSound;

    private void Start()
    {
        ClearCanvas();

        canvas[0].gameObject.SetActive(true);
    }

    public void ClearCanvas()
    {
        for (int i = 0; i < canvas.Count; i++)
        {
            canvas[i].gameObject.SetActive(false);
        }
    }


    public void ChangeScene(string scene)
    {
        StartCoroutine(PlayAndWait(scene));
    }

    public void PlayClicSound()
    {
        Sound s = AudioManager.instance.GetSound(buttonSound);
        AudioManager.instance.Play(buttonSound);
    }


    private IEnumerator PlayAndWait(string scene)
    {
        Sound s = AudioManager.instance.GetSound(buttonSound);
        AudioManager.instance.Play(buttonSound);

        PlayClicSound();
        yield return new WaitForSeconds(s.clip.length);
        SceneManager.LoadScene(scene);
    }

    public void QuiGame()
    {
        StartCoroutine(QuitGameAndWait());
    }

    private IEnumerator QuitGameAndWait()
    {
        Sound s = AudioManager.instance.GetSound(buttonSound);
        AudioManager.instance.Play(buttonSound);


        yield return new WaitForSeconds(s.clip.length);
        Application.Quit();
    }

}
