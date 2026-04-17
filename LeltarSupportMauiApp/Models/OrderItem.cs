using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LeltarSupportMauiApp.Models
{
    public class OrderItem : INotifyPropertyChanged
    {
        private int _id;
        private int? _orderId;
        private int? _productId;
        private int _quantity;
        private Order? _order;
        private Product? _product;

        public int Id
        {
            get => _id;
            set
            {
                if (_id == value) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public int? OrderId
        {
            get => _orderId;
            set
            {
                if (_orderId == value) return;
                _orderId = value;
                OnPropertyChanged();
            }
        }

        public int? ProductId
        {
            get => _productId;
            set
            {
                if (_productId == value) return;
                _productId = value;
                OnPropertyChanged();
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity == value) return;
                _quantity = value;
                OnPropertyChanged();
            }
        }

        // Navigation
        public Order? Order
        {
            get => _order;
            set
            {
                if (_order == value) return;
                _order = value;
                OnPropertyChanged();
            }
        }

        public Product? Product
        {
            get => _product;
            set
            {
                if (_product == value) return;
                _product = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}