namespace OrderMauiApp.Models
{
    // Junction table between products and ingredients
    public class ProductIngredient
    {
        public int ProductId { get; set; }
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; }

        // Navigation
        public Product? Product { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}