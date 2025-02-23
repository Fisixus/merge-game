using UnityEngine;

namespace Core.GridEffects
{
    public class MergeGlowParticle : MonoBehaviour
    {
        [field: SerializeField] public ParticleSystem MergeGlowParticleSystem { get; private set; }

        private ParticleSystem.MainModule _particleSettings;

        private void Awake()
        {
            _particleSettings = MergeGlowParticleSystem.main;
        }

        public void SetColor(Color color)
        {
            _particleSettings.startColor = new ParticleSystem.MinMaxGradient(color);
        }

        public float Play()
        {
            var duration = MergeGlowParticleSystem.main.duration;
            MergeGlowParticleSystem.Play();
            return duration;
        }
    }
}
