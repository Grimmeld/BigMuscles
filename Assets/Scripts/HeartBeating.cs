using UnityEngine;

public class HeartBeating : MonoBehaviour
{
    [SerializeField] private Transform heart;
    [SerializeField] private float pace;
    [SerializeField] private int count;

    private void OnEnable()
    {
        float delay = Random.Range(3, 7);

        Invoke("Beating", delay);
        
    }

    private void OnDisable()
    {
        BackToNormal();
        CancelInvoke();
    }

    private void Beating()
    {
        CancelInvoke();
        float rate = Random.Range(3, 20);
        heart.LeanScale(new Vector3(1.05f, 1.05f, 1.05f), pace).setEaseInOutBack().setLoopCount(count).setOnComplete(BackToNormal);
        Invoke("Beating", rate);
        

    }

    private void BackToNormal()
    {
        heart.LeanScale(new Vector3(1f, 1f, 1f), pace).setEaseInBack();

    }
}
