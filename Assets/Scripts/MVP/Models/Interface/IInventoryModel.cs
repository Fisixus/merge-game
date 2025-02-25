using System.Collections.Generic;
using Core.Inventories;

namespace MVP.Models.Interface
{
     public interface IInventoryModel
     {
          IReadOnlyList<InventoryPawn> Pawns { get; }
          void AddPawn(InventoryPawn pawn);
          void RemovePawn(InventoryPawn pawn);
     }
}
