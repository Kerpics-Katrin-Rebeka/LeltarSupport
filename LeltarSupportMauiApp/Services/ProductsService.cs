using LeltarSupportMauiApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeltarSupportMauiApp.Services
{
    internal class ProductsService
    {
        List<Product> _products = new List<Product>();

        public ProductsService() 
        {
            this._products = GetProductsAsync().Result as List<Product>;
        }


        public Task<IEnumerable<Product>> GetProductsAsync()
        {
            return DataService.SelectAsync<Product>("api/products");
        }

        //public Task UpdateInventory(int productId, int quantity)
        //{
        //    try
        //    {
        //        Product? selectedProduct = _products.Find(p => p.Id == productId);
        //        Ingredient?[] ingredients = _products.Find(p => p.Id == productId)?.ProductIngredients.Select(pi => pi.Ingredient).ToArray() ?? Array.Empty<Ingredient>();
        //        if (selectedProduct != null)
        //        {
        //            var tasks = new List<Task>();
        //            for (int i = 0; i < ingredients!.Length; i++)
        //            {
        //                tasks.Add(DataService.PutAsync<int, Inventory>($"api/inventory/{ingredients[i].Id}/adjust", quantity));
        //            }
        //            return Task.WhenAll(tasks);
        //        }
        //        return Task.CompletedTask;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Task.CompletedTask;
        //    }
        //}
    }
}
