using LeltarSupportMauiApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeltarSupportMauiApp.Services
{
    internal class ProductsService
    {
        public ProductsService() { }


        public Task<IEnumerable<Product>> GetProductsAsync()
        {
            return DataService.SelectAsync<Product>("api/products");
        }
    }
}
