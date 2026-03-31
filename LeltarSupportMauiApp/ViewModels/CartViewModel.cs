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
        public ICommand RemoveFromCartCommand { get; }

        public CartViewModel()
        {
            AddToCartCommand = new Command<Product>(AddToCart);
            RemoveFromCartCommand = new Command<CartItem>(RemoveFromCart);

        }

        private void AddToCart(Product product)
        {
            if (product == null) return;

            var existingItem = CartItems.FirstOrDefault(i => i.Product.Name == product.Name);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                CartItems.Add(new CartItem
                {
                    Product = product,
                    Quantity = 1
                });
            }
        }

        private void RemoveFromCart(CartItem item)
        {
            if (item == null) return;
            var existingItem = CartItems.FirstOrDefault(i => i.Product.Name == item.Product.Name);
            if (existingItem != null)
            {
                existingItem.Quantity--;
                if (existingItem.Quantity <= 0)
                {
                    CartItems.Remove(existingItem);
                }
            }
        }
    }
}
