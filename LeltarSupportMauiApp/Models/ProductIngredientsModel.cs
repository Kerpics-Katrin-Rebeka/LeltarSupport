using Newtonsoft.Json;

namespace LeltarSupportMauiApp.Models
{
    public class ProductIngredientsModel
    {
        [JsonProperty("productId")]
        public int ProductId { get; set; }
        [JsonProperty("ingredientId")]
        public int IngredientId { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
