namespace EnhanzerAssignment.API.DTOs
{
    public class CreatePurchaseOrderDto
    {
        public List<CreatePurchaseOrderItemDto> Items { get; set; }
        = new();
    }
}
