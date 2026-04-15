using System.Collections.Generic;

namespace LeltarSupportMauiApp.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;

        // Navigation
        public IList<ProductIngredient> ProductIngredients { get; set; } = new List<ProductIngredient>();
        public Inventory? Inventory { get; set; }
        public IList<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
        public IList<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
    }
}