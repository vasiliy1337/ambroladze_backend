using ambroladze_backend.DTO;
using ambroladze_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Versioning;
using System.Security.Claims;

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
        public async Task<ActionResult<IEnumerable<OrderOutDTO>>> GetOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            if (User == null)
            {
                return Unauthorized();
            }
            List<OrderOutDTO> orders = new List<OrderOutDTO>();
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            int id;
            int.TryParse(User.FindFirst("id")?.Value, out id);
            if (role == "admin")
            {
                orders = (from o in _context.Orders
                                        select new OrderOutDTO()
                                        {
                                            Id = o.Id,
                                            Address = o.Address,
                                            ClientName = o.Client.Name,
                                            TypeName = o.TypeOfWork.Name,
                                            DateOfEnd = o.DateOfEnd,
                                            DateOfStart = o.DateOfStart
                                        }).ToList();
            }
            else
            {
                orders = (from o in _context.Orders where o.ClientId == id
                          select new OrderOutDTO()
                          {
                              Id = o.Id,
                              Address = o.Address,
                              ClientName = o.Client.Name,
                              TypeName = o.TypeOfWork.Name,
                              DateOfEnd = o.DateOfEnd,
                              DateOfStart = o.DateOfStart
                          }).ToList();
            }

            
            //var orders = await _context.Orders.Include(o => o.TypeOfWork).Include(o => o.Client).Select(new OrderOutDTO() { Id = })
            

            return orders;
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
        [Authorize(Roles = "admin")]
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("sum/{id}")]
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

        public class ForTop
        {
            public int id { get; set; }
            public int count { get; set; }
        }

        [HttpGet("top")]
        public async Task<ActionResult<List<ForTop>>> GetTop()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            List <ForTop> result = await _context.Orders.Include(o => o.TypeOfWork).GroupBy(o => o.TypeOfWorkId)
                  .OrderByDescending(gp => gp.Count())
                  .Take(5).Select(g => new ForTop { id = g.Key, count = g.Count() }).ToListAsync();

            return result;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("OrdersByClient/")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<List<Order>>> GetOrdersByClientId(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            List <Order> result = new List<Order>();
            if (User.FindFirst("id")?.Value == id.ToString()) {
                result = _context.Orders.Where(o => o.ClientId == id).ToList();
            }


            return result;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("ClientsByType/")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<List<Client>>> GetClientsByTypeId(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            var result = _context.Orders.Include(o => o.Client).Where(o => o.TypeOfWorkId == id).Select(o => o.Client)
                .GroupBy(x => x.Id).Select(g => g.First()).ToList();

            return result;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("TypesByClient/")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<List<TypeOfWork>>> GetTypesByClientId(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            var result = _context.Orders.Include(o => o.TypeOfWork).Where(o => o.ClientId == id).Select(o => o.TypeOfWork)
                .GroupBy(x => x.Id).Select(g => g.First()).ToList();

            return result;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("json/")]
        public async Task<ActionResult<IEnumerable<OrderOutDTO>>> GetOrdersForFront()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            //var orders = await _context.Orders.Include(o => o.TypeOfWork).Include(o => o.Client).Select(new OrderOutDTO() { Id = })
            List<OrderOutDTO> orders = (from o in _context.Orders
                                        select new OrderOutDTO()
                                        {
                                            Id = o.Id,
                                            Address = o.Address,
                                            ClientName = o.Client.Name,
                                            TypeName = o.TypeOfWork.Name,
                                            DateOfEnd = o.DateOfEnd,
                                            DateOfStart = o.DateOfStart
                                        }).ToList();

            return orders;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///

        [HttpGet("sumbydate/")]
        public async Task<ActionResult<double>> SumByDate(DateTime start, DateTime end)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            double totalCost = _context.Orders.Include(o => o.TypeOfWork).ToList().Where(o => o.DateOfEnd>=start && o.DateOfEnd<=end).Sum(o => o.TypeOfWork.Cost);

            return totalCost;
        }


    }
}
