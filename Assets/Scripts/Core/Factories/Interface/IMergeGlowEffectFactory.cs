using AYellowpaper.SerializedCollections;
using Core.GridEffects;
using UnityEngine;

namespace Core.Factories.Interface
{
    public interface IMergeGlowEffectFactory : IFactory<MergeGlowParticle>
    {
        SerializedDictionary<ColorType, Color> MergeGlowEffectColorDict { get; }
        MergeGlowParticle GenerateMergeGlowEffect(ColorType colorType);
    }
}