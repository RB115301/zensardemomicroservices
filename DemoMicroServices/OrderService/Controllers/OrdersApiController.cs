using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Messaging;
using OrderService.Models;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersApiController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly OrderPublisher _orderPublisher;
        private readonly HttpClient _httpClient;

        public OrdersApiController(OrderDbContext context, OrderPublisher orderPublisher)
        {
            _context = context;
            _orderPublisher = orderPublisher;
            _httpClient = new HttpClient();
        }

        [HttpGet("product/{id}")]
        public async Task<Product> GetProductAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"http://apigateway:8080/products/{id}");
            response.EnsureSuccessStatusCode();
            var product = await response.Content.ReadFromJsonAsync<Product>();
            return product!;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        [HttpGet("{id")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(o => o.Id == id);
            if(order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            if(ModelState.IsValid)
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                await _orderPublisher.PublishOrderAsync(order);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            else
            {
                return BadRequest(order);
            }
        }
    }
}
