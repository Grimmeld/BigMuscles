using UnityEngine;

public class ButtonBox : MonoBehaviour
{

    private void OnEnable()
    {
        transform.localPosition = new Vector2(-Screen.width, 240);
        transform.LeanMoveX(100, 0.5f).setEaseOutExpo().delay = 0.3f;
    }

    private void OnDisable()
    {
        transform.LeanMoveX(-Screen.width, 0.5f).setEaseInExpo();
    }

}
