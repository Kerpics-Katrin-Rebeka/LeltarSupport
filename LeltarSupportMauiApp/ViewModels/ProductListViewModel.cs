using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeltarSupportMauiApp.DataServices;
using LeltarSupportMauiApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeltarSupportMauiApp.ViewModels
{
    public partial class ProductListViewModel: ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ProductsModel> productList = new ObservableCollection<ProductsModel>();

        [ObservableProperty]
        private ProductsModel selectedProduct;

        [RelayCommand]
        private async void ProductDetails()
        {
            var navigationParameters = new Dictionary<string, object>() {
                { "products", selectedProduct  }
            };
            await Shell.Current.GoToAsync("details", navigationParameters);
        }

        public ProductListViewModel()
        {
            getProducts();
            Console.WriteLine(productList);
        }

        public async void getProducts()
        {
            ProductList.Clear();
            IEnumerable<ProductsModel> list = await DataService.select("/products");
            var reversedList = list.Reverse();
            foreach (var item in reversedList)
            {
                ProductList.Add(item);
            }
        }
    }
}
