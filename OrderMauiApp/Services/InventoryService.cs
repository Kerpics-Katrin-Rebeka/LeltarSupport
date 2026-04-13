using LeltarSupportMauiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMauiApp.Services
{
    internal class InventoryService
    {
        public async Task<IEnumerable<Inventory>> LoadInventory()
        {
            var inventory = await DataService.SelectAsync<Inventory>("api/inventory").ConfigureAwait(false);
            return inventory?.ToList() ?? Enumerable.Empty<Inventory>();
        }
    }
}
