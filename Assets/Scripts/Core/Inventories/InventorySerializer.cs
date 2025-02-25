using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core.Inventories
{
    public static class InventorySerializer
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "inventory.json");
    
        
        //  Save inventory to JSON
        public static void SaveInventory(List<InventoryPawn> items)
        {
            string json = JsonUtility.ToJson(new InventoryData(items), true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"Inventory saved to: {SavePath}");
        }
    
        //  Load inventory from JSON
        public static List<InventoryPawn> LoadInventory()
        {
            if (!File.Exists(SavePath))
            {
                Debug.LogWarning("No inventory save file found!");
                return new List<InventoryPawn>();
            }

            string json = File.ReadAllText(SavePath);
            InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(json);
            return inventoryData.Items;
        }
    }
    
    // ✅ Serializable wrapper for storing list of inventory items
    [System.Serializable]
    public class InventoryData
    {
        public List<InventoryPawn> Items;

        public InventoryData(List<InventoryPawn> items)
        {
            Items = items;
        }
    }
}
