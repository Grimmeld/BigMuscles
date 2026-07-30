using UnityEngine;

public class MeterSetUp : MonoBehaviour
{
    private void Awake()
    {
        CharacterManagement.Instance.SetMeterContainer(this.gameObject.transform);
    }
}
