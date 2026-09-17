namespace EnhanzerAssignment.API.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        public decimal NetAmount { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<PurchaseOrderItem> Items { get; set; }
            = new List<PurchaseOrderItem>();
    }
}
