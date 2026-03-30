using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeltarSupportMauiApp.Models;
using LeltarSupportMauiApp.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LeltarSupportMauiApp.ViewModels
{
    public partial class ProductListViewModel : ObservableObject
    {
        private readonly ProductsService _productservice = new();
        [ObservableProperty]
        private ObservableCollection<Product> productList = new ObservableCollection<Product>();

        [ObservableProperty]
        private Product selectedProduct;

        public ProductListViewModel()
        {
            _ = LoadProductsAsync();
        }

        [RelayCommand]
        private async Task LoadProductsAsync()
        {
            try
            {
                ProductList.Clear();
                var list = await _productservice.GetProductsAsync().ConfigureAwait(false);
                foreach (Product item in list)
                {
                    ProductList.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadProductsAsync error: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task ProductDetails()
        {
            if (SelectedProduct == null)
                return;

            var navigationParameters = new Dictionary<string, object>()
            {
                { "product", SelectedProduct }
            };

            await Shell.Current.GoToAsync("details", navigationParameters).ConfigureAwait(false);
        }
    }
}
