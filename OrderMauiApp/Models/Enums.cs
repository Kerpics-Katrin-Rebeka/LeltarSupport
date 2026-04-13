using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OrderMauiApp.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StockMovementReason
    {
        order,
        restock,
        manual,
        correction
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PurchaseOrderStatus
    {
        recommended,
        ordered,
        received
    }
}