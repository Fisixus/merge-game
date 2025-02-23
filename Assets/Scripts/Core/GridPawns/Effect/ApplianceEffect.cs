using DG.Tweening;
using UnityEngine;

namespace Core.GridPawns.Effect
{
    public class ApplianceEffect : GridPawnEffect
    {
        [field: SerializeField] public ParticleSystem GlowEffect { get; private set; }


        public void SetGlowing(bool isGlowingOn)
        {
            GlowEffect.transform.DOKill();
            if (isGlowingOn)
            {
                GlowEffect.Play();
                GlowEffect.transform.DOScale(1f, 0.15f);
            }
            else
            {
                GlowEffect.transform.DOScale(0f, 0.15f)
                    .OnComplete(() =>
                    {
                        GlowEffect.Stop();
                    });
            }
        }
        
    }
}
