using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeltarSupportMauiApp.Models;
using Microsoft.Maui.Controls;
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

        [RelayCommand]
        private async Task LoadInventoryAsync()
        {
            try
            {
                InventoryList.Clear();
                var list = await _inventoryService.LoadInventory();

                foreach (var item in list)
                {
                    InventoryList.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadInventoryAsync error: {ex.Message}");
            }
        }



    }
}
