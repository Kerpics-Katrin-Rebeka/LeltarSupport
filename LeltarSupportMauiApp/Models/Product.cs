using System.Collections.Generic;

namespace LeltarSupportMauiApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public bool Active { get; set; } = true;

        // Navigation
        public IList<ProductIngredient> ProductIngredients { get; set; } = new List<ProductIngredient>();
        public IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}