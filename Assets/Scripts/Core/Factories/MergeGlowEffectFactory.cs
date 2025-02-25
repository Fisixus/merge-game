using AYellowpaper.SerializedCollections;
using Core.Factories.Interface;
using Core.Factories.Pools;
using Core.GridEffects;
using UnityEngine;

namespace Core.Factories
{
    public class MergeGlowEffectFactory : ObjectFactory<MergeGlowParticle>, IMergeGlowEffectFactory
    {
        [field: SerializeField]
        [SerializedDictionary("Color Type", "Color")]
        public SerializedDictionary<ColorType, Color> MergeGlowEffectColorDict { get; private set; }

        public override void PreInitialize()
        {
            Pool = new ObjectPool<MergeGlowParticle>(ObjPrefab, ParentTr, 8);
        }

        public MergeGlowParticle GenerateMergeGlowEffect(ColorType colorType)
        {
            var mergeGlowEffect = CreateObj();
            mergeGlowEffect.SetColor(MergeGlowEffectColorDict[colorType]);
            return mergeGlowEffect;
        }
    }
}