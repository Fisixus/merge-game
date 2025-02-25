using System;

namespace Core.GridSerialization
{
    [Serializable]
    public class GridJson
    {
        public int grid_width = 8; // Fixed grid size
        public int grid_height = 8;
        public JsonPawn[] grid; // Stores all grid items
    }

    [Serializable]
    public class JsonPawn
    {
        public string pawn_type; // Example: "ApplianceA", "ProducerB"
        public int level; // Level of the pawn
        public int capacity;
    }
}