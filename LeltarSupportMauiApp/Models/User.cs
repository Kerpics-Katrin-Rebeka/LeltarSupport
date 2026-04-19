using System;
using System.Collections.Generic;

namespace LeltarSupportMauiApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public IList<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public IList<Order> Orders { get; set; } = new List<Order>();
    }
}