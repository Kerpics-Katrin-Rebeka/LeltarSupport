using Newtonsoft.Json;

namespace LeltarSupportMauiApp.Models
{
    public class IngredientModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("quantity")]
        public double Quantity { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
