using UnityEngine;

public class HeartBeats : MonoBehaviour
{
    [SerializeField] private Transform heart;
    [SerializeField] private float pace;
    [SerializeField] private int count;

    private void OnEnable()
    {
        heart.LeanScale(new Vector3(1.1f, 1.1f, 1.1f), pace).setEaseInBack().setLoopCount(count).setOnComplete(BackToNormal);
    }

    private void BackToNormal()
    {
        heart.LeanScale(new Vector3(1f, 1f, 1f), pace).setEaseInBack();

    }
}
