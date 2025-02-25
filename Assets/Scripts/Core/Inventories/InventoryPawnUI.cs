using Core.GridPawns;
using Core.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Inventories
{
    public class InventoryPawnUI : MonoBehaviour
    {
        [field:SerializeField] public Button PawnButton { get;  private set; }
        public InventoryPawn InventoryPawn { get; set; } = new InventoryPawn();

        public void FillData(GridPawn activePawn)
        {
            InventoryPawn.Type = activePawn.Type;
            InventoryPawn.Level = activePawn.Level;
            InventoryPawn.Sprite = activePawn.SpriteRenderer.sprite;
        }
    }
}
