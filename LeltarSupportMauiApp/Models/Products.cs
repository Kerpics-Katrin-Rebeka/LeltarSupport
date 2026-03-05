using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeltarSupportMauiApp.Models
{
    public interface Products
    {
        int Id { get; set; }
        string Name { get; set; }
        double Price { get; set; }
        bool Active { get; set; }
    }
}
