using EnhanzerAssignment.API.Data;
using EnhanzerAssignment.API.DTOs;
using EnhanzerAssignment.API.Models;
using EnhanzerAssignment.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnhanzerAssignment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPurchaseOrderService _purchaseOrderService;
        public PurchaseOrderController(ApplicationDbContext context, IPurchaseOrderService purchaseOrderService)
        {
            _context = context;
            _purchaseOrderService = purchaseOrderService;
        }

        // POST: api/PurchaseOrder
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(
            CreatePurchaseOrderDto dto)
        {
            if (dto.Items == null ||
                dto.Items.Count == 0)
            {
                return BadRequest(
                    "Purchase order must contain at least one item.");
            }

            var order = await _purchaseOrderService.CreateAsync(dto);

            return Ok(new
            {
                message = "Purchase order saved successfully.",
                id = order.Id,
                netAmount = order.NetAmount
            });
        }

        // GET: api/PurchaseOrder/latest
        [HttpGet("latest")]
        [Authorize]
        public async Task<IActionResult> GetLatest()
        {
            var orders =
                await _purchaseOrderService.GetLatestAsync();

            return Ok(orders.Select(x => new
            {
                id = x.Id,
                netAmount = x.NetAmount,
                noOfItems = x.Items.Count
            }));
        }

        // GET: api/PurchaseOrder/oldest-items
        [HttpGet("oldest-items")]
        [Authorize]
        public async Task<IActionResult> GetOldestItems()
        {
            var items =
                await _purchaseOrderService.GetOldestItemsAsync();

            return Ok(items.Select(x => new
            {
                purchaseOrderId = x.PurchaseOrderId,
                itemName = x.ItemName,
                quantity = x.Quantity
            }));
        }

        [HttpGet("item-quantities")]
        [Authorize]
        public async Task<IActionResult> GetItemQuantities()
        {
            var result =
                await _purchaseOrderService.GetItemQuantitiesAsync();

            return Ok(result);
        }
    }
}
