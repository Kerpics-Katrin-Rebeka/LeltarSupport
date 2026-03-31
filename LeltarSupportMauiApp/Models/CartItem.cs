using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LeltarSupportMauiApp.Models
{ 
    public class CartItem : PurchaseOrderItem, INotifyPropertyChanged
    {
        private Product _product = null!;

        public new int Id
        {
            get => base.Id;
            set
            {
                if (base.Id != value)
                {
                    base.Id = value;
                    OnPropertyChanged();
                }
            }
        }

        public Product Product
        {
            get => _product;
            set
            {
                if (_product != value)
                {
                    _product = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }
        public new int Quantity
        {
            get => (int)(base.Quantity ?? 0m);
            set
            {
                var current = base.Quantity ?? 0m;
                if (current != value)
                {
                    base.Quantity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        public decimal TotalPrice => (Product?.Price ?? 0m) * Quantity;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
