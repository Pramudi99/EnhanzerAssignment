namespace EnhanzerAssignment.API.DTOs
{
    public class CreatePurchaseOrderItemDto
    {
        public string ItemName { get; set; }
        = string.Empty;

        public int Quantity { get; set; }

        public decimal StandardCost { get; set; }

        public decimal StandardPrice { get; set; }

        public decimal Discount { get; set; }
    }
}
