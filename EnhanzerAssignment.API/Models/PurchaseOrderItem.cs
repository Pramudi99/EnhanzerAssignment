namespace EnhanzerAssignment.API.Models
{
    public class PurchaseOrderItem
    {
        public int Id { get; set; }

        public int PurchaseOrderId { get; set; }

        public string ItemName { get; set; }
            = string.Empty;

        public int Quantity { get; set; }

        public decimal StandardCost { get; set; }

        public decimal StandardPrice { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalCost { get; set; }

        public decimal TotalSelling { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }
            = null!;
    }
}
