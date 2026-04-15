using System;

namespace OrderMauiApp.Models
{
    public class StockMovement
    {
        public int Id { get; set; }
        public int IngredientId { get; set; }
        public decimal? ChangeAmount { get; set; }
        public StockMovementReason? Reason { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Ingredient? Ingredient { get; set; }
    }
}