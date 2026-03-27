using UnityEngine;

public class EnemyVFXController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;

    public void Play()
    {
        if (_particle == null)
            return;

        if (!_particle.isPlaying)
            _particle.Play();
    }

    public void Stop()
    {
        if (_particle == null)
            return;

        _particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}