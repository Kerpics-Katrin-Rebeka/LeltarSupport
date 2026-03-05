using LeltarSupportMauiApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeltarSupportMauiApp.DataServices
{
    internal class DataService
    {

        static string url = "http://localhost:3000";
        public static async Task<IEnumerable<ProductsModel>> select(string route)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
            var uri = route;
                var result = await client.GetStringAsync(uri);
                return JsonConvert.DeserializeObject<List<ProductsModel>>(result);
            }
        }
    }
}
