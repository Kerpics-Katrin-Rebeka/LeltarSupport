using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace OrderMauiApp.Models
{
    public partial class Inventory : ObservableObject
    {
        [ObservableProperty]
        private int ingredientId;

        [ObservableProperty]
        private decimal quantity;

        [ObservableProperty]
        private decimal minimumLevel;

        [ObservableProperty]
        private decimal changeQuantity = 1m;

        [ObservableProperty]
        private decimal pendingAdjustment;

        public Ingredient? Ingredient { get; set; }

        public bool IsLowStock => Quantity <= MinimumLevel;

        partial void OnQuantityChanged(decimal value)
        {
            OnPropertyChanged(nameof(IsLowStock));
        }

        partial void OnMinimumLevelChanged(decimal value)
        {
            OnPropertyChanged(nameof(IsLowStock));
        }
    }
}