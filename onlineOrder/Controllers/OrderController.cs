using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onlineOrder.Data;
using onlineOrder.DTOs;
using onlineOrder.Models;
using System.Collections.Generic;
using System.Linq;

namespace onlineOrder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ECommerceContext _context;

        public OrderController(ECommerceContext context)
        {
            _context = context;
        }

        // GET: api/order
        [HttpGet]
        public ActionResult<IEnumerable<OrderDto>> GetOrders()
        {
            var orders = _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ToList();

            var dtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                CreatedAt = o.CreatedAt,
                Items = o.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            }).ToList();

            return Ok(dtos);
        }

        // GET: api/order/5
        [HttpGet("{id}")]
        public ActionResult<OrderDto> GetOrder(int id)
        {
            var order = _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null) return NotFound();

            var dto = new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            return Ok(dto);
        }

        // POST: api/order
        [HttpPost]
        public ActionResult<OrderDto> CreateOrder([FromBody] OrderDto orderDto)
        {
            var order = new Order
            {
                UserId = orderDto.UserId,
                CreatedAt = DateTime.Now,
                Items = orderDto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            // 返回 DTO
            orderDto.Id = order.Id;
            orderDto.CreatedAt = order.CreatedAt;
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, orderDto);
        }

        // PUT: api/order/5
        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] OrderDto orderDto)
        {
            var order = _context.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);

            if (order == null) return NotFound();

            // 更新基本信息
            order.UserId = orderDto.UserId;

            // 更新 Items
            // 简单处理：先删除旧的，再加新的
            _context.OrderItems.RemoveRange(order.Items);

            order.Items = orderDto.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/order/5
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _context.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);

            if (order == null) return NotFound();

            _context.OrderItems.RemoveRange(order.Items);
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
