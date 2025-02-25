using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Inventories
{
    [System.Serializable]
    public class InventoryPawn
    {
        [NonSerialized]public string ID;
        public Enum Type;
        public int Level;
        public Sprite Sprite; // UI-independent, but supports UI later

        public InventoryPawn(){}
        public InventoryPawn(Enum type, int level, Sprite icon)
        {
            ID = Guid.NewGuid().ToString(); //  Returns a unique string ID
            Type = type;
            Level = level;
            Sprite = icon;
        }
    }
}
