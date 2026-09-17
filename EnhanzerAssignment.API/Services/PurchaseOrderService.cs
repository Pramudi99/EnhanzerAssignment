using EnhanzerAssignment.API.Data;
using EnhanzerAssignment.API.DTOs;
using EnhanzerAssignment.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EnhanzerAssignment.API.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly ApplicationDbContext _context;
        public PurchaseOrderService(ApplicationDbContext context) { 
            _context = context;
        }

        public async Task<PurchaseOrder> CreateAsync(
        CreatePurchaseOrderDto dto)
        {
            var order = new PurchaseOrder
            {
                CreatedAt = DateTime.UtcNow
            };

            foreach (var itemDto in dto.Items)
            {
                var totalCost =
                    itemDto.StandardCost *
                    itemDto.Quantity *
                    (1 - itemDto.Discount / 100);

                var totalSelling =
                    itemDto.StandardPrice *
                    itemDto.Quantity;

                var item = new PurchaseOrderItem
                {
                    ItemName = itemDto.ItemName,
                    Quantity = itemDto.Quantity,
                    StandardCost = itemDto.StandardCost,
                    StandardPrice = itemDto.StandardPrice,
                    Discount = itemDto.Discount,
                    TotalCost = totalCost,
                    TotalSelling = totalSelling
                };

                order.Items.Add(item);
            }

            order.NetAmount =
                order.Items.Sum(x => x.TotalSelling);

            _context.PurchaseOrders.Add(order);

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<List<PurchaseOrder>> GetLatestAsync()
        {
            return await _context.PurchaseOrders
                .Include(x => x.Items)
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .ToListAsync();
        }

        public async Task<List<PurchaseOrderItem>>
    GetOldestItemsAsync()
        {
            return await _context.PurchaseOrderItems
                .OrderBy(x => x.Id)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<ItemQuantityDto>>
    GetItemQuantitiesAsync()
        {
            return await _context.PurchaseOrderItems
                .GroupBy(x => x.ItemName)
                .Select(g => new ItemQuantityDto
                {
                    ItemName = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .ToListAsync();
        }


    }
}
