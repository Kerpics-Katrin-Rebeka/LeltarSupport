using System;
using System.Collections.Generic;

namespace LeltarSupportMauiApp.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public int? SupplierId { get; set; }
        public PurchaseOrderStatus? Status { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Supplier? Supplier { get; set; }
        public IList<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
    }
}