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
    public partial class InventoryModel : ObservableObject
    {
        private readonly InventoryService _inventoryService = new InventoryService();

        [ObservableProperty]
        private Inventory inventoryList = new Inventory();

        public InventoryModel() { }

        [RelayCommand]
        private async Task LoadInventoryAsync()
        {
            try
            {
                InventoryList.Clear();
                var list = await _inventoryService.StartOrderAsync();

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
