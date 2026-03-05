using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LeltarSupportMauiApp.Models
{
    public class ProductsModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("price")]
        public double Price { get; set; }
        
        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
