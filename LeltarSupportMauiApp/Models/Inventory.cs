namespace LeltarSupportMauiApp.Models
{
    public class Inventory
    {
        // ingredient_id is primary key and foreign key to ingredients.id
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; }
        public decimal MinimumLevel { get; set; }

        // Navigation
        public Ingredient? Ingredient { get; set; }
    }
}