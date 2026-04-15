using LeltarSupportMauiApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeltarSupportMauiApp.Services
{
    class OrderService
    {
        public async Task Order(OrderItem[] items)
        {
            if (items == null || items.Length == 0)
                throw new InvalidOperationException("Cannot place an order with an empty cart.");

            var payload = new CreateOrderRequest
            {
                Items = items
                    .Select(i => new CreateOrderItemRequest
                    {
                        ProductId = i.ProductId ?? i.Product?.Id ?? 0,
                        Quantity = i.Quantity
                    })
                    .Where(i => i.ProductId > 0 && i.Quantity > 0)
                    .ToArray()
            };

            if (payload.Items.Length == 0)
                throw new InvalidOperationException("No valid items were found in the cart.");

            await DataService.PostAsync<CreateOrderRequest, Order>("api/orders", payload).ConfigureAwait(false);
        }

        private sealed class CreateOrderRequest
        {
            [JsonProperty("items")]
            public CreateOrderItemRequest[] Items { get; set; } = Array.Empty<CreateOrderItemRequest>();
        }

        private sealed class CreateOrderItemRequest
        {
            [JsonProperty("product_id")]
            public int ProductId { get; set; }

            [JsonProperty("quantity")]
            public int Quantity { get; set; }
        }
    }
}
