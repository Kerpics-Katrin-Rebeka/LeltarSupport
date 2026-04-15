using System.Collections.Generic;

namespace OrderMauiApp.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Contact { get; set; }

        // Navigation
        public IList<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    }
}