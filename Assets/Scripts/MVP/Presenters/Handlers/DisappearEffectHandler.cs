using System;
using Core.Factories.Interface;
using Core.GridEffects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MVP.Presenters.Handlers
{
    public class DisappearEffectHandler
    {
        private readonly IDisappearEffectFactory _disappearEffectFactory;

        public DisappearEffectHandler(IDisappearEffectFactory disappearEffectFactory)
        {
            _disappearEffectFactory = disappearEffectFactory;
        }
        
        public async UniTaskVoid PlayDisappearEffect(Vector3 worldPos, ColorType colorType)
        {
            DisappearParticle disappearParticle = _disappearEffectFactory.GenerateDisappearEffect(colorType);
            disappearParticle.transform.position = worldPos;
            var duration = disappearParticle.Play();
            await UniTask.Delay(TimeSpan.FromSeconds(duration), DelayType.DeltaTime);
            _disappearEffectFactory.DestroyObj(disappearParticle);
            
        }
    }
}
