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
            var jsonItems = ConvertToInventoryJson(items);
            string json = JsonUtility.ToJson(new InventoryData(jsonItems), true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"Inventory saved to: {SavePath}");
        }

        private static List<InventoryPawnJson> ConvertToInventoryJson(List<InventoryPawn> items)
        {
            throw new System.NotImplementedException();
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
            var items = ConvertToInventoryPawn(inventoryData.Items);
            return items;
        }

        private static List<InventoryPawn> ConvertToInventoryPawn(List<InventoryPawnJson> inventoryDataItems)
        {
            throw new System.NotImplementedException();
        }
    }
    
    // ✅ Serializable wrapper for storing list of inventory items
    [System.Serializable]
    public class InventoryData
    {
        public List<InventoryPawnJson> Items;

        public InventoryData(List<InventoryPawnJson> items)
        {
            Items = items;
        }
    }
}
