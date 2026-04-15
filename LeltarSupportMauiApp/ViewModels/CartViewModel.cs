using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LeltarSupportMauiApp.Models;
using LeltarSupportMauiApp.Services;

namespace LeltarSupportMauiApp.ViewModels;

public partial class CartViewModel : ObservableObject
{
    [ObservableProperty]
    public ObservableCollection<OrderItem> orderItems = new ObservableCollection<OrderItem>();

    [ObservableProperty]
    private decimal sumPrice;

    public ICommand AddToCartCommand { get; }
    public ICommand RemoveFromCartCommand { get; }
    public ICommand PurchaseCommand { get; }
    public ICommand ClearCartCommand { get; }

    OrderService orderService = new OrderService();

    public CartViewModel()
    {
        AddToCartCommand = new Command<Product>(AddToCart);
        RemoveFromCartCommand = new Command<OrderItem>(RemoveFromCart);
        PurchaseCommand = new Command(Purchase);
        ClearCartCommand = new Command(CleanCart);
    }

    private void AddToCart(Product product)
    {
        if (product == null) return;

        var existingItem = OrderItems.FirstOrDefault(i => i.Product?.Id == product.Id || i.ProductId == product.Id);

        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            OrderItems.Add(new OrderItem
            {
                Product = product,
                ProductId = product.Id,
                Quantity = 1
            });
        }

        RecalculateTotal();
    }

    private void RemoveFromCart(OrderItem item)
    {
        if (item == null) return;

        var itemProductId = item.ProductId ?? item.Product?.Id;
        if (!itemProductId.HasValue) return;

        var existingItem = OrderItems.FirstOrDefault(i => (i.ProductId ?? i.Product?.Id) == itemProductId.Value);

        if (existingItem != null)
        {
            existingItem.Quantity--;

            if (existingItem.Quantity <= 0)
                OrderItems.Remove(existingItem);
        }

        RecalculateTotal();
    }

    private async void Purchase()
    {
        try
        {
            await orderService.Order(OrderItems.ToArray());
            CleanCart();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Purchase error: {ex.Message}");
        }
    }

    private void RecalculateTotal()
    {
        SumPrice = OrderItems.Sum(i => (i.Product?.Price ?? 0m) * i.Quantity);
    }

    private void CleanCart()
    {
        OrderItems.Clear();
        RecalculateTotal();
    }
}