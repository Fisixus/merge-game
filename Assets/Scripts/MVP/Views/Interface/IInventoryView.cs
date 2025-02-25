using System.Collections.Generic;
using Core.Inventories;
using MVP.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace MVP.Views.Interface
{
    public interface IInventoryView
    {
        CanvasGroup InventoryCanvasGroup { get;  } 
        InventoryButton InventoryButton { get;  }
        Button InventoryCloseButton { get;  }
        Transform InventoryPawnUIsParentTr { get; }
        void PlayInventoryButtonParticle();
        void OpenInventoryPanel();
        void SubscribeInventoryButton(InventoryPresenter inventoryPresenter);
        List<InventoryPawnUI> InventoryPawnUIs { get; }
    }
}
