using UnityEngine;

public class ParticleUI : MonoBehaviour
{
    [SerializeField] private Transform bonusPoint;
    [SerializeField] private ParticleSystem bonusParticle;
    [SerializeField] private Transform malusPoint;
    [SerializeField] private ParticleSystem malusParticle;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        CharacterManagement.Instance.OnValueChanged += ActivateParticleBrometer;

    }

    private void ActivateParticleBrometer(float newMeter, float bonus)
    {
        ParticleSystem particle = null;
        if (bonus > 0)
        {
            particle = Instantiate(bonusParticle, bonusPoint);
            Destroy(particle.gameObject, particle.main.duration);
        }
        else if(bonus < 0) 
        {
            particle = Instantiate(malusParticle, malusPoint);
            Destroy(particle.gameObject, particle.main.duration);
        }
        else
        { }
    }
}
