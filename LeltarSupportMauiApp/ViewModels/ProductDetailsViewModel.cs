using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeltarSupportMauiApp.Models;
using LeltarSupportMauiApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        private ProductsModel selectedProduct;
        [ObservableProperty]
        private IngredientModel selectedIngredient;

        public ProductDetailsViewModel()
        {
            getIngredients();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query != null && query.ContainsKey("products") && query["products"] is ProductsModel prod)
            {
                SelectedProduct = prod;
            }
        }
        partial void OnSelectedProductChanged(ProductsModel value)
        {
            if (value != null)
            {
                getIngredientsByProductId(value.Id);
            }
        }

        public async void getIngredients()
        {
            Ingredients.Clear();
            IEnumerable<IngredientModel> list = await DataService.SelectAsync<IngredientModel>("/ingredients");
            var reversedList = list.Reverse();
            foreach (var item in reversedList)
            {
                Ingredients.Add(item);
            }
        }

        public async void getIngredientsByProductId(int id)
        {
            ProductIngredients.Clear();
            IEnumerable<ProductIngredientsModel> list = await DataService.SelectAsync<ProductIngredientsModel>("/product_ingredients");
            var reversedList = list.Reverse();
            foreach (var item in reversedList)
            {
                if (item.ProductId == id)
                {
                    ProductIngredients.Add(item);
                }
            }
        }

        [RelayCommand]
        private async Task AddIngredientAsync()
        {
            if (SelectedProduct == null)
                return;

            var ingredient = SelectedIngredient;
            if (ingredient == null)
                return;

            var newLink = new ProductIngredientsModel
            {
                ProductId = SelectedProduct.Id,
                IngredientId = ingredient.Id,
                Quantity = 1 
            };

            try
            {
                await DataService.SelectAsync<ProductIngredientsModel>("/product_ingredients");
                getIngredientsByProductId(SelectedProduct.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddIngredientAsync error: {ex.Message}");
            }
        }
    }
}
