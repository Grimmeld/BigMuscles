using UnityEngine;

public class MeterSetUp : MonoBehaviour
{
    private void Start()
    {
        CharacterManagement.Instance.SetMeterContainer(this.gameObject.transform);
    }
}
