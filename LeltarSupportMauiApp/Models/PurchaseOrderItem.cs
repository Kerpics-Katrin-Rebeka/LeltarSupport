namespace LeltarSupportMauiApp.Models
{
    public class PurchaseOrderItem
    {
        public int Id { get; set; }
        public int? PurchaseOrderId { get; set; }
        public int? IngredientId { get; set; }
        public decimal? Quantity { get; set; }

        public PurchaseOrder? PurchaseOrder { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}