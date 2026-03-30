using CommunityToolkit.Mvvm.ComponentModel;
using LeltarSupportMauiApp.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace LeltarSupportMauiApp.ViewModels
{
    public partial class CartViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<CartItem> cartItems = new ObservableCollection<CartItem>();

        public ICommand AddToCartCommand { get; }

        public CartViewModel()
        {
            AddToCartCommand = new Command<Product>(AddToCart);
            System.Diagnostics.Debug.WriteLine($"CartViewModel ctor hash={this.GetHashCode()}, initial Count={CartItems?.Count}");
        }

        private void AddToCart(Product product)
        {
            System.Diagnostics.Debug.WriteLine($"CartViewModel.AddToCart -> {product?.Name}");
            if (product == null) return;

            var existingItem = CartItems.FirstOrDefault(i => i.Product.Name == product.Name);

            if (existingItem != null)
            {
                existingItem.Quantity++;
                System.Diagnostics.Debug.WriteLine($"CartViewModel: incremented existing item '{product?.Name}' -> Quantity={existingItem.Quantity}");
            }
            else
            {
                CartItems.Add(new CartItem
                {
                    Product = product,
                    Quantity = 1
                });
                System.Diagnostics.Debug.WriteLine($"CartViewModel: added new item '{product?.Name}' -> Count={CartItems.Count}");
            }
        }
    }
}
