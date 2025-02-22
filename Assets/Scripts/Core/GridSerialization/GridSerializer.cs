using System;
using System.IO;
using Core.GridPawns;
using Core.GridPawns.Enum;
using UnityEngine;

namespace Core.GridSerialization
{
    public static class GridSerializer
    {
        private static readonly string PersistentPath = Path.Combine(Application.persistentDataPath, "grid_data.json");

        public static GridInfo SerializeToGridInfo()
        {
            if (!File.Exists(PersistentPath)) return null;
            
            string jsonString = File.ReadAllText(PersistentPath);
            var gridJson = JsonUtility.FromJson<GridJson>(jsonString);
            var (gridPawnTypes, gridPawnLevels) =  ProcessGridJson(gridJson);
            return new GridInfo(gridPawnTypes, gridPawnLevels);

        }

        public static void SaveGrid(GridPawn[,] grid)
        {
            try
            {
                var columnCount = grid.GetLength(0);
                var rowCount = grid.GetLength(1);
                JsonPawn[,] jsonPawn = new JsonPawn[columnCount, rowCount];
                for (int i = 0; i < columnCount; i++)
                {
                    for (int j = 0; j < rowCount; j++)
                    {
                        JsonPawn pawn = new JsonPawn
                        {
                            pawn_type = grid[i, j].ToString(),
                            level = grid[i, j].Level
                        };
                        jsonPawn[i, j] = pawn;
                    }
                }

                var gridJson = ConvertToGridJson(columnCount, rowCount, jsonPawn);
                string json = JsonUtility.ToJson(gridJson, true);
                File.WriteAllText(PersistentPath, json);
                Debug.Log("JSON saved to: " + PersistentPath);
            }
            catch (Exception e)
            {
                Debug.Log("JSON error:" + e);
                throw;
            }
        }
        
        public static (Enum[,] gridPawnTypes, int[,] gridPawnLevels) ProcessGridJson(GridJson gridJson)
        {
            
            var gridPawnTypes = new Enum[gridJson.grid_height, gridJson.grid_width];
            var gridPawnLevels = new int[gridJson.grid_height, gridJson.grid_width];

            int gridIndex = 0;
            for (int i = 0; i < gridJson.grid_height; i++)
            for (int j = 0; j < gridJson.grid_width; j++)
            {
                gridPawnLevels[i, j] = gridJson.grid[gridIndex].level;
                gridPawnTypes[i, j] = ConvertJsonToPawnType(gridJson.grid[gridIndex].pawn_type);
                gridIndex++;
            }

            return (gridPawnTypes, gridPawnLevels);
        }
        
        private static Enum ConvertJsonToPawnType(string pawnString)
        {
            switch (pawnString)
            {
                case nameof(JsonGridPawnType.empty):
                    return ApplianceType.None;
                case nameof(JsonGridPawnType.productA):
                    return ApplianceType.ApplianceA;
                case nameof(JsonGridPawnType.producerA):
                    return ProducerType.ProducerA;
                default:
                    return ApplianceType.None;
            }
        }
        
        private static JsonGridPawnType ConvertPawnTypeToJson(Enum gridPawnType)
        {
            switch (gridPawnType)
            {
                case ApplianceType.ApplianceA:
                    return JsonGridPawnType.productA;
                case ProducerType.ProducerA:
                    return JsonGridPawnType.producerA;
                default:
                    return JsonGridPawnType.empty;
            }
        }
        
        public static GridJson ConvertToGridJson(int gridWidth, int gridHeight, JsonPawn[,] pawns)
        {
            GridJson gridJson = new GridJson
            {
                grid_width = gridWidth,
                grid_height = gridHeight,
                grid = new JsonPawn[gridWidth * gridHeight],
            };

            for (int x = 0; x < gridHeight; x++)
            {
                for (int y = 0; y < gridWidth; y++)
                {
                    int index = (x * gridWidth) + y; // Correct indexing from 0 to (gridHeight * gridWidth - 1)
                    gridJson.grid[index] = pawns[x, y];
                }
            }
            return gridJson;
        }
        
    }
}
