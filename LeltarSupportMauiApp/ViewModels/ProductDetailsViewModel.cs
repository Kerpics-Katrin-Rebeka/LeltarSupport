using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeltarSupportMauiApp.Models;
using LeltarSupportMauiApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LeltarSupportMauiApp.ViewModels
{
    public partial class ProductDetailsViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        private ObservableCollection<IngredientModel> ingredients = new ObservableCollection<IngredientModel>();

        [ObservableProperty]
        private ObservableCollection<ProductIngredientsModel> productIngredients = new ObservableCollection<ProductIngredientsModel>();

        [ObservableProperty]
        private Product selectedProduct;

        [ObservableProperty]
        private IngredientModel selectedIngredient;

        public ProductDetailsViewModel()
        {
            _ = LoadIngredientsAsync();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query != null && query.ContainsKey("product") && query["product"] is Product prod)
            {
                SelectedProduct = prod;
                _ = LoadProductDetailsAsync(prod.Id);
            }
        }

        partial void OnSelectedProductChanged(Product value)
        {
            if (value != null)
            {
                _ = LoadProductDetailsAsync(value.Id);
            }
        }

        private async Task LoadIngredientsAsync()
        {
            try
            {
                Ingredients.Clear();
                var list = await DataService.SelectAsync<IngredientModel>("api/ingredients").ConfigureAwait(false);
                var reversed = list?.Reverse() ?? Enumerable.Empty<IngredientModel>();
                foreach (var item in reversed)
                    Ingredients.Add(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadIngredientsAsync error: {ex.Message}");
            }
        }

        private async Task LoadProductDetailsAsync(int id)
        {
            try
            {
                ProductIngredients.Clear();

                var product = await DataService.SelectSingleAsync<Product>($"api/products/{id}").ConfigureAwait(false);
                if (product == null)
                    return;

                var allLinks = await DataService.SelectAsync<ProductIngredientsModel>("api/product_ingredients").ConfigureAwait(false);
                foreach (var link in allLinks.Where(x => x.ProductId == id))
                    ProductIngredients.Add(link);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadProductDetailsAsync error: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task AddIngredientAsync()
        {
            if (SelectedProduct == null || SelectedIngredient == null)
                return;

            var newLink = new ProductIngredientsModel
            {
                ProductId = SelectedProduct.Id,
                IngredientId = SelectedIngredient.Id,
                Quantity = 1
            };

            try
            {
                var created = await DataService.PostAsync<ProductIngredientsModel, ProductIngredientsModel>("api/product_ingredients", newLink).ConfigureAwait(false);
                if (created != null)
                {
                    ProductIngredients.Add(created);
                }
                else
                {
                    await LoadProductDetailsAsync(SelectedProduct.Id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddIngredientAsync error: {ex.Message}");
            }
        }
    }
}
