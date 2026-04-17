using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LeltarSupportMauiApp.Models;
using LeltarSupportMauiApp.Services;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System;
using System.Collections.Generic;

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
        PurchaseCommand = new Command(Purchase, CanPurchase);
        ClearCartCommand = new Command(CleanCart);

        OrderItems.CollectionChanged += OrderItems_CollectionChanged;
    }

    private void AddToCart(Product product)
    {
        // Defensive checks and logging to avoid creating invalid OrderItems
        if (product == null)
        {
            Debug.WriteLine("AddToCart called with null product.");
            return;
        }

        if (product.Id <= 0)
        {
            Debug.WriteLine($"AddToCart: product has invalid Id={product.Id}, Name='{product?.Name ?? "(null)"}'. Aborting add.");
            return;
        }

        var existingItem = OrderItems.FirstOrDefault(i => i.Product?.Id == product.Id || i.ProductId == product.Id);

        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            var newItem = new OrderItem
            {
                Product = product,
                ProductId = product.Id,
                Quantity = 1
            };

            AttachItemHandler(newItem);
            OrderItems.Add(newItem);
        }

        RecalculateTotal();
    }

    private async void RemoveFromCart(OrderItem item)
    {
        if (item == null) return;

        var itemProductId = item.ProductId ?? item.Product?.Id;
        if (!itemProductId.HasValue) return;

        var existingItem = OrderItems.FirstOrDefault(i => (i.ProductId ?? i.Product?.Id) == itemProductId.Value);

        if (existingItem != null)
        {
            existingItem.Quantity--;

            if (existingItem.Quantity <= 0)
            {
                DetachItemHandler(existingItem);
                OrderItems.Remove(existingItem);
            }
        }
        RecalculateTotal();            
    }

    private async void Purchase()
    {
        if(OrderItems.FirstOrDefault().Product == null)
        {
            Console.WriteLine("Cart is empty. Cannot proceed with purchase.");
            return;
        }
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
        ((Command)PurchaseCommand).ChangeCanExecute();
    }

    private void CleanCart()
    {
        foreach (var it in OrderItems.ToList())
            DetachItemHandler(it);

        OrderItems.Clear();
        RecalculateTotal();
    }

    private bool CanPurchase() => OrderItems.Any();

    private void OrderItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        var badItems = new List<OrderItem>();

        if (e.NewItems != null)
        {
            foreach (OrderItem item in e.NewItems.Cast<OrderItem>())
            {
                // If an OrderItem with both Product and ProductId null appears, remove and log stack trace
                if (item.Product == null && item.ProductId == null)
                {
                    badItems.Add(item);
                    Debug.WriteLine("OrderItems_CollectionChanged: Detected OrderItem with null Product and null ProductId. Removing it.");
                    Debug.WriteLine(Environment.StackTrace);
                    continue;
                }

                AttachItemHandler(item);
            }
        }

        if (e.OldItems != null)
        {
            foreach (OrderItem item in e.OldItems.Cast<OrderItem>())
                DetachItemHandler(item);
        }

        // Remove any bad items after iteration to avoid modifying collection while enumerating NewItems
        foreach (var bi in badItems)
        {
            DetachItemHandler(bi);
            if (OrderItems.Contains(bi))
                OrderItems.Remove(bi);
        }

        ((Command)PurchaseCommand).ChangeCanExecute();
        RecalculateTotal();
    }

    private void AttachItemHandler(OrderItem item)
    {
        if (item is INotifyPropertyChanged inpc)
            inpc.PropertyChanged += OrderItem_PropertyChanged;
    }

    private void DetachItemHandler(OrderItem item)
    {
        if (item is INotifyPropertyChanged inpc)
            inpc.PropertyChanged -= OrderItem_PropertyChanged;
    }

    private void OrderItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(OrderItem.Quantity) ||
            e.PropertyName == nameof(OrderItem.Product))
        {
            RecalculateTotal();
            ((Command)PurchaseCommand).ChangeCanExecute();
        }
    }
}