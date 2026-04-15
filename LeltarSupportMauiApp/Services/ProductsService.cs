using LeltarSupportMauiApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeltarSupportMauiApp.Services
{
    internal class ProductsService
    {
        public async Task<IEnumerable<Product>> StartOrderAsync()
        {
            var products = await DataService.SelectAsync<Product>("api/products").ConfigureAwait(false);
            return products?.ToList() ?? [];
        }
    }
}
