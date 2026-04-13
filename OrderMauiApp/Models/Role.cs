using System.Collections.Generic;

namespace OrderMauiApp.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Navigation
        public IList<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}