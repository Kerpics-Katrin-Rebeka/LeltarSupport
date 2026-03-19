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
        [ObservableProperty]
        private ObservableCollection<ProductsModel> productList = new ObservableCollection<ProductsModel>();

        [ObservableProperty]
        private ProductsModel selectedProduct;

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
                var list = await DataService.SelectAsync<ProductsModel>("api/products").ConfigureAwait(false);
                var reversed = list?.Reverse() ?? Enumerable.Empty<ProductsModel>();
                foreach (var item in reversed)
                    ProductList.Add(item);
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
