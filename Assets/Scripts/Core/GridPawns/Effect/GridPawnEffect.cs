using DG.Tweening;
using UnityEngine;

namespace Core.GridPawns.Effect
{
    public class GridPawnEffect : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer FocusSprite { get; private set; }

        public void SetFocus(float scale)
        {
            transform.DOKill();
            FocusSprite.transform.DOKill();
            transform.localScale = Vector3.one;

            FocusSprite.transform.DOScale(scale, 0.15f);
            if(scale >= 1f)
                transform.DOScale(0.9f, 0.15f).SetLoops(2, LoopType.Yoyo);
        }

    }
}
