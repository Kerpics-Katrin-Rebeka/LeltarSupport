using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OrderMauiApp.Models;
using OrderMauiApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderMauiApp.ViewModels
{
    public partial class InventoryViewModel : ObservableObject
    {
        private readonly InventoryService _inventoryService = new InventoryService();

        [ObservableProperty]
        private ObservableCollection<Inventory> inventoryList = new ObservableCollection<Inventory>();

        public ICommand DecreaseCommand { get; }
        public ICommand IncreaseCommand { get; }
        public ICommand SaveCommand { get; }

        public InventoryViewModel()
        {
            DecreaseCommand = new AsyncRelayCommand<Inventory>(DecreaseInventoryAsync);
            IncreaseCommand = new AsyncRelayCommand<Inventory>(IncreaseInventoryAsync);
            SaveCommand = new AsyncRelayCommand(SaveInventoryAsync);
        }

        [RelayCommand]
        private async Task LoadInventoryAsync()
        {
            try
            {
                InventoryList.Clear();
                var list = await _inventoryService.LoadInventory();

                foreach (var item in list)
                {
                    item.PendingAdjustment = 0;
                    item.ChangeQuantity = item.ChangeQuantity <= 0 ? 1m : item.ChangeQuantity;
                    InventoryList.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadInventoryAsync error: {ex.Message}");
            }
        }

        private Task DecreaseInventoryAsync(Inventory item)
        {
            if (item is null)
                return Task.CompletedTask;

            var amount = item.ChangeQuantity <= 0 ? 1m : item.ChangeQuantity;
            var appliedAmount = Math.Min(amount, item.Quantity);

            item.Quantity -= appliedAmount;
            item.PendingAdjustment -= appliedAmount;

            return Task.CompletedTask;
        }

        private Task IncreaseInventoryAsync(Inventory item)
        {
            if (item is null)
                return Task.CompletedTask;

            var amount = item.ChangeQuantity <= 0 ? 1m : item.ChangeQuantity;

            item.Quantity += amount;
            item.PendingAdjustment += amount;

            return Task.CompletedTask;
        }

        private async Task SaveInventoryAsync()
        {
            try
            {
                foreach (var item in InventoryList)
                {
                    if (item.PendingAdjustment == 0)
                        continue;

                    await DataService.PostAsync<decimal, object>(
                        $"api/inventory/{item.IngredientId}/adjust",
                        item.PendingAdjustment);

                    item.PendingAdjustment = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SaveInventoryAsync error: {ex.Message}");
            }
        }
    }
}
