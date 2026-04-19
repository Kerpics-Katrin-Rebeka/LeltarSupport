using System.Collections.Generic;

namespace LeltarSupportMauiApp.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public IList<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}