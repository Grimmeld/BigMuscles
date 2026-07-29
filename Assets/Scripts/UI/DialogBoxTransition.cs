using UnityEngine;

public class DialogBoxTransition : MonoBehaviour
{
    public Transform box;

    private void OnEnable()
    {
        box.localPosition = new Vector2(Screen.width, 0);
        box.LeanMoveLocalX(0, 0.5f).setEaseOutExpo().delay = 0.1f;
    }

    public void CloseDialog()
    {
        box.LeanMoveLocalX(Screen.width, 0.5f).setEaseInExpo();
    }
}
