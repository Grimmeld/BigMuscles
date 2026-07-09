using UnityEngine;

public class ParticleUI : MonoBehaviour
{
    [SerializeField] private ParticleSystem bonusParticle;
    [SerializeField] private ParticleSystem malusParticle;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        CharacterManagement.Instance.OnValueChanged += ActivateParticleBrometer;

    }

    private void ActivateParticleBrometer(float newMeter, float bonus)
    {

        
        if (bonus > 0)
        {
            bonusParticle.Stop();
            bonusParticle.Play();
        }
        else if(bonus < 0) 
        {
            malusParticle.Stop();
            malusParticle.Play();
        }
        else
        { }
    }
}
