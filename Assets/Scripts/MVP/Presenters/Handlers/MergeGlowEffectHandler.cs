using System;
using Core.Factories.Interface;
using Core.GridEffects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MVP.Presenters.Handlers
{
    public class MergeGlowEffectHandler
    {
        private readonly IMergeGlowEffectFactory _mergeGlowEffectFactory;

        public MergeGlowEffectHandler(IMergeGlowEffectFactory mergeGlowEffectFactory)
        {
            _mergeGlowEffectFactory = mergeGlowEffectFactory;
        }

        public async UniTaskVoid PlayMergeGlowEffect(Vector3 worldPos, ColorType colorType)
        {
            MergeGlowParticle mergeGlowParticle = _mergeGlowEffectFactory.GenerateMergeGlowEffect(colorType);
            mergeGlowParticle.transform.position = worldPos;
            var duration = mergeGlowParticle.Play();
            await UniTask.Delay(TimeSpan.FromSeconds(duration), DelayType.DeltaTime);
            _mergeGlowEffectFactory.DestroyObj(mergeGlowParticle);
        }
    }
}