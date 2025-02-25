using AYellowpaper.SerializedCollections;
using Core.Factories.Interface;
using Core.Factories.Pools;
using Core.GridEffects;
using UnityEngine;

namespace Core.Factories
{
    public class DisappearEffectFactory : ObjectFactory<DisappearParticle>, IDisappearEffectFactory
    {
        [field: SerializeField]
        [SerializedDictionary("Color Type", "Color")]
        public SerializedDictionary<ColorType, Color> DisappearEffectColorDict { get; private set; }

        public override void PreInitialize()
        {
            Pool = new ObjectPool<DisappearParticle>(ObjPrefab, ParentTr, 8);
        }

        public DisappearParticle GenerateDisappearEffect(ColorType colorType)
        {
            var disappearEffect = CreateObj();
            disappearEffect.SetColor(DisappearEffectColorDict[colorType]);
            return disappearEffect;
        }
    }
}