namespace LeltarSupportMauiApp.Models
{
    public class Inventory
    {
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; }
        public decimal MinimumLevel { get; set; }

        public Ingredient? Ingredient { get; set; }
        public bool IsLowStock => Quantity <= MinimumLevel;
    }
}