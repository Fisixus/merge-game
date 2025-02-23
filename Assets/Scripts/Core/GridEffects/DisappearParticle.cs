using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearParticle : MonoBehaviour
{
    [field: SerializeField] public ParticleSystem DisappearParticleSystem { get; private set; }

    private ParticleSystem.MainModule _particleSettings;

    private void Awake()
    {
        _particleSettings = DisappearParticleSystem.main;
    }

    public void SetColor(Color color)
    {
        _particleSettings.startColor = new ParticleSystem.MinMaxGradient(color);
    }

    public float Play()
    {
        var duration = DisappearParticleSystem.main.duration;
        DisappearParticleSystem.Play();
        return duration;
    }
}
