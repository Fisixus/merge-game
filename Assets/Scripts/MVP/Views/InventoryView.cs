using System.Collections.Generic;
using Core.Inventories;
using DG.Tweening;
using MVP.Presenters;
using MVP.Views.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace MVP.Views
{
    public class InventoryView : MonoBehaviour, IInventoryView
    {
        
        [field: SerializeField]public CanvasGroup InventoryCanvasGroup { get; private set; } 
        [field: SerializeField]public InventoryButton InventoryButton { get; private set; }
        [field: SerializeField]public Button InventoryCloseButton { get; private set; }
        
        [field: SerializeField]public Transform InventoryPawnUIsParentTr { get; private set; } // Parent UI panel

        public List<InventoryPawnUI> InventoryPawnUis { get; private set; }

        private void OnEnable()
        {
            InventoryCloseButton.onClick.AddListener(CloseInventoryPanel);
            InventoryPawnUis = new List<InventoryPawnUI>();
        }

        private void OnDisable()
        {
            InventoryButton.Button.onClick.RemoveAllListeners();
            InventoryCloseButton.onClick.RemoveAllListeners();
        }
        public void SubscribeInventoryButton(InventoryPresenter inventoryPresenter)
        {
            InventoryButton.Button.onClick.AddListener(()=> inventoryPresenter.OnInventoryRequested());
        }

        public void OpenInventoryPanel()
        {
            InventoryCanvasGroup.DOKill();
            InventoryCanvasGroup.blocksRaycasts = true;
            DOTween.To(() => InventoryCanvasGroup.alpha, x => InventoryCanvasGroup.alpha = x, 1, 0.5f)
                .OnComplete(() =>
                {
                    InventoryCanvasGroup.interactable = true;
                });
        }
        
        public void CloseInventoryPanel()
        {
            InventoryCanvasGroup.DOKill();
            InventoryCanvasGroup.interactable = false;
            InventoryCanvasGroup.blocksRaycasts = false;
            DOTween.To(() => InventoryCanvasGroup.alpha, x => InventoryCanvasGroup.alpha = x, 0, 0.5f);
        }

        

    }
}
