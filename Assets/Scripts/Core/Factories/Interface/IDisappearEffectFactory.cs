using AYellowpaper.SerializedCollections;
using Core.GridEffects;
using UnityEngine;

namespace Core.Factories.Interface
{
    public interface IDisappearEffectFactory : IFactory<DisappearParticle>
    {
        SerializedDictionary<ColorType, Color> DisappearEffectColorDict { get; }
        DisappearParticle GenerateRocketExplosionEffect(ColorType colorType);
    }
}
