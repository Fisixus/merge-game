using System;
using DG.Tweening;
using UnityEngine;

namespace Core.GridPawns.Effect
{
    public class GridPawnEffect : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer FocusSprite { get; private set; }
        [field: SerializeField] public SpriteRenderer LastLevelSprite { get; private set; }
        private Tween _shiftTween;
        
        public void SetLastLevel(float scale)
        {
            LastLevelSprite.transform.DOKill();
            LastLevelSprite.transform.DOScale(scale, 0.15f);
        }
        
        public void SetFocus(float scale)
        {
            transform.DOKill();
            FocusSprite.transform.DOKill();
            transform.localScale = Vector3.one;

            FocusSprite.transform.DOScale(scale, 0.15f);
            if(scale >= 1f)
                transform.DOScale(0.9f, 0.15f).SetLoops(2, LoopType.Yoyo);
        }

        public void Shift(Vector3 position, Action a, float animTime=0.3f)
        {
            _shiftTween?.Kill(); // Ensure only one tween sequence is active
            _shiftTween = transform.DOMove(position, animTime).SetEase(Ease.OutQuad).OnComplete(a.Invoke);
        }
        
    }
}
