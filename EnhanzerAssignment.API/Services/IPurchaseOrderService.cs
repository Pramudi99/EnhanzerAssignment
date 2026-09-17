using EnhanzerAssignment.API.DTOs;
using EnhanzerAssignment.API.Models;

namespace EnhanzerAssignment.API.Services
{
    public interface IPurchaseOrderService
    {
        Task<PurchaseOrder> CreateAsync(
        CreatePurchaseOrderDto dto);

        Task<List<PurchaseOrder>> GetLatestAsync();

        Task<List<PurchaseOrderItem>> GetOldestItemsAsync();

        Task<List<ItemQuantityDto>> GetItemQuantitiesAsync();
    }
}
