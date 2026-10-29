using UnityEngine;

public class ParticlesTimer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float timer = 1f;
    [SerializeField] private int particlesPerBurst ;

    private float currentTime;

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timer)
        {
            _particleSystem.Emit(particlesPerBurst);
            currentTime -= timer;
        }
    }
}