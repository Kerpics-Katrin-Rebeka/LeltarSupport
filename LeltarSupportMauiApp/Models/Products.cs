using Newtonsoft.Json;

namespace LeltarSupportMauiApp.Models
{
    public class ProductsModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("price")]
        public double Price { get; set; }
        
        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
