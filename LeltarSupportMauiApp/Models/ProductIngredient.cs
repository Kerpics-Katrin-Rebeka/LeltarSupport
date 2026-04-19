namespace LeltarSupportMauiApp.Models
{
    public class ProductIngredient
    {
        public int ProductId { get; set; }
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; }

        public Product? Product { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}