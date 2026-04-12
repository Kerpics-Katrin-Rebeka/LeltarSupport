using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeltarSupportMauiApp.Models;
using LeltarSupportMauiApp.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Product> productList = new();

        public ProductListViewModel(CartViewModel cartViewModel)
        {
            _cartViewModel = cartViewModel;
            CartCommand = new Command(OpenCart);
        }

        [RelayCommand]
        private async Task LoadProductsAsync()
        {
            try
            {
                ProductList.Clear();
                var list = await _productservice.StartOrderAsync();

                foreach (var item in list)
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
            await Shell.Current.GoToAsync("cart");
        }

        public void AddToCartExecute(Product product)
        {
            if (product == null) return;
            _cartViewModel.AddToCartCommand.Execute(product);
        }

        public void CleanCart()
        {
            _cartViewModel.ClearCartCommand.Execute(null);

        }
    }
}
