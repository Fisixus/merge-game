using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Inventories
{
    public class InventoryButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [field:SerializeField] public Button Button { get; private set; }
        public bool IsEntered { get; set; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsEntered = true;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            IsEntered = false;
        }
    }
}
