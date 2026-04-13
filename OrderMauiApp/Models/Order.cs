using System;
using System.Collections.Generic;

namespace OrderMauiApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public User? User { get; set; }
        public IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}