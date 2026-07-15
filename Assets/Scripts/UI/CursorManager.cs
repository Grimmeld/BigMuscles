using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D onHoverCursor;
    public static CursorManager Instance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;

        if (onHoverCursor == null)
            Debug.LogError("[UIManager] Texture Cursor on hover is missing");

    }

    public Texture2D GetOnHoverCursor()
    {
        return onHoverCursor;
    }

}
