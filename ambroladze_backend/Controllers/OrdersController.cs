using ambroladze_backend.DTO;
using ambroladze_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography;

namespace ambroladze_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrdersController : ControllerBase
    {
        private readonly OrderContext _context;

        public OrdersController(OrderContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderDTO orderDTO)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'Context.Orders'  is null.");
            }
            Order order = new Order(orderDTO);

            var user = await _context.Clients.FindAsync(order.ClientId);
            var tp = await _context.TypesOfWork.FindAsync(order.TypeOfWorkId);
            if (tp == null || user == null) { return NotFound(); }

            order.DateOfEnd = order.DateOfStart.AddDays(tp.Duration);

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("/sum/{id}")]
        public async Task<ActionResult<double>> GetSumForUser(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            double totalCost = _context.Orders.Include(o => o.TypeOfWork).ToList().Where(o => o.ClientId == id).Sum(o => o.TypeOfWork.Cost);

            return totalCost;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        //[HttpGet("/top")]
        //public async Task<ActionResult<Order>> GetTop()
        //{
        //    if (_context.Orders == null)
        //    {
        //        return NotFound();
        //    }

        //    //var orderTop = _context.Orders.Select(o => o.TypeOfWorkId).Join(_context.Orders.Include(o => o.TypeOfWork).Select(o => o.TypeOfWork.Name).Count());

        //    //var orderTop = 

        //    return orderTop;
        //}

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////


        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
