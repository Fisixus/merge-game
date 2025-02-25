using System;
using Core.GridPawns;
using Core.Tasks;
using DG.Tweening;
using MVP.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Inventories
{
    public class InventoryPawnUI : MonoBehaviour
    {
        [field:SerializeField] public Button PawnButton { get;  private set; }
        public InventoryPawn InventoryPawn { get; set; }
        
        private Sequence _shakeSeq;

        private void OnDisable()
        {
            PawnButton.onClick.RemoveAllListeners();
        }

        public void SubscribeToInventoryPawnUIClick(InventoryPresenter inventoryPresenter)
        {
            PawnButton.onClick.AddListener(()=>inventoryPresenter.OnPawnRequestedFromInventory(InventoryPawn));
        }

        public void FillData(GridPawn activePawn)
        {
            InventoryPawn = new InventoryPawn(activePawn.Type, activePawn.Level);
            SetPawnUISprite(activePawn.SpriteRenderer.sprite);
        }

        public void SetPawnUISprite(Sprite sprite)
        {
            PawnButton.image.sprite = sprite;
        }

        public void Shake(float duration = 0.05f)
        {
            _shakeSeq?.Kill();
            _shakeSeq = DOTween.Sequence()
                .Append(transform.DORotate(new Vector3(0f, 0f, -45f), duration).SetEase(Ease.OutQuad))
                .Append(transform.DORotate(new Vector3(0f, 0f, 0f), duration).SetEase(Ease.InQuad))
                .Append(transform.DORotate(new Vector3(0f, 0f, 45f), duration).SetEase(Ease.InQuad))
                .Append(transform.DORotate(new Vector3(0f, 0f, 0f), duration).SetEase(Ease.OutQuad));
        }
    }
}
