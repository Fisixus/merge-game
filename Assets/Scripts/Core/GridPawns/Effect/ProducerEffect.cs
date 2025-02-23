using System;
using DG.Tweening;
using UnityEngine;

namespace Core.GridPawns.Effect
{
    public class ProducerEffect : GridPawnEffect
    {
        [field: SerializeField] public SpriteRenderer CapacitySprite { get; private set; }

        private Tween _capacityScaleTween;

        private void OnEnable()
        {
            StartScaling();
        }

        private void OnDisable()
        {
            StopScaling();
        }

        private void StartScaling()
        {
            _capacityScaleTween?.Kill();
            _capacityScaleTween = CapacitySprite.transform.DOScale(Vector3.one * 1.2f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void StopScaling()
        {
            if (_capacityScaleTween == null) return;
            _capacityScaleTween.Kill();
            CapacitySprite.transform.localScale = Vector3.one;
        }
    
    
    
    }
}
