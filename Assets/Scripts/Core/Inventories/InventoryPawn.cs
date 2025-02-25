using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Inventories
{
    [System.Serializable]
    public class InventoryPawn
    {
        public string ID;
        public Enum Type;
        public int Level;

        public InventoryPawn(){}
        public InventoryPawn(Enum type, int level)
        {
            ID = Guid.NewGuid().ToString(); //  Returns a unique string ID
            Type = type;
            Level = level;
        }
        public override string ToString()
        {
            return $"InventoryPawnUI - Type: {Type}, Level: {Level}";
        }
    }
}
