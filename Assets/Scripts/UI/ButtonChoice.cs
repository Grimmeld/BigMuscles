using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChoice : MonoBehaviour
{

    private Button button;
    [SerializeField] private Image imageLock;
    public bool isLocked;

    private void Awake()
    {
        imageLock.gameObject.SetActive(false);
        button = GetComponent<Button>();
        isLocked = false;
    }

    public void OnLocked()
    {
        button.interactable = false;
        imageLock.gameObject.SetActive(true);
    }

}
