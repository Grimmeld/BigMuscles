using SimpleTwineDialogue;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance;
    [SerializeField] private List<GameObject> prefab;
    [SerializeField] private GameObject currentChapter;
    [SerializeField] private TextAdventure currentText;
    [SerializeField] private int currentIdx;


    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;

        if (prefab == null)
        {
            Debug.LogError("No scenes is set up");
        }
    }

    private void Start()
    {
        currentIdx = 0;
        
        NewTextAdventure();
    }


    public void SetNewScene()
    {
        ClearTextAdventure();
        
        currentIdx++;

        if (currentIdx < prefab.Count)
        {
            // Put some effects before scene

            NewTextAdventure();
        }
        else
        {
            //finish game;
        }
        
    }

    private void ClearTextAdventure()
    {

            CharacterManagement.Instance.StopAllCoroutines();
            CharacterManagement.Instance.SetMeterContainer(null);
            currentText.OnEndChapter -= SetNewScene;
            Destroy(currentText);
            Destroy(currentChapter);

    }

    private void NewTextAdventure()
    {
            currentChapter = Instantiate(prefab[currentIdx]);
            currentText = currentChapter.GetComponentInChildren<TextAdventure>();
            currentText.OnEndChapter += SetNewScene;
    }
}
