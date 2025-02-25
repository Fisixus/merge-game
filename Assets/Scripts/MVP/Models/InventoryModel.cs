using System.Collections.Generic;
using Core.Inventories;
using MVP.Models.Interface;
using UnityEngine;

namespace MVP.Models
{
    public class InventoryModel : IInventoryModel
    {
        private List<InventoryPawn> _pawns;

        public IReadOnlyList<InventoryPawn> Pawns => _pawns;

        public InventoryModel()
        {
            //TODO:_pawns = InventorySerializer.LoadInventory(); //  Load saved inventory
        }

        public void AddPawn(InventoryPawn pawn)
        {
            _pawns.Add(pawn);
            InventorySerializer.SaveInventory(_pawns); //  Auto-save after change
        }

        public void RemovePawn(InventoryPawn pawn)
        {
            _pawns.Remove(pawn);
            InventorySerializer.SaveInventory(_pawns); //  Auto-save after change
        }
    }
}
