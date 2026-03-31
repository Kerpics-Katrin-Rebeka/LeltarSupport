using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeltarSupportMauiApp.Models;
using LeltarSupportMauiApp.Services;
using LeltarSupportMauiApp.Views;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LeltarSupportMauiApp.ViewModels
{
    public partial class ProductListViewModel : ObservableObject
    {
        private readonly ProductsService _productservice = new();
        private readonly CartViewModel _cartViewModel;
        public ICommand CartCommand { get; }

        [ObservableProperty]
        private ObservableCollection<Product> productList = new ObservableCollection<Product>();

        public ProductListViewModel(CartViewModel cartViewModel)
        {
            _cartViewModel = cartViewModel;
            _ = LoadProductsAsync();
            CartCommand = new Command(OpenCart);
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
        private async void OpenCart()
        {
            await Shell.Current.GoToAsync("cart").ConfigureAwait(false);
        }

        public void AddToCartExecute(Product product)
        {
            if(product == null) return;
            _cartViewModel.AddToCartCommand.Execute(product);
        }
    }
}
