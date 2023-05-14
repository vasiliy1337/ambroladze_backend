using ambroladze_backend.DTO;
using ambroladze_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ambroladze_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeOfWorkController : ControllerBase
    {
        private readonly OrderContext _context;

        public TypeOfWorkController(OrderContext context)
        {
            _context = context;
        }

        // GET: api/TypesOfWork
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeOfWork>>> GetTypeOfWork()
        {
            if (_context.TypesOfWork == null)
            {
                return NotFound();
            }
            return await _context.TypesOfWork.ToListAsync();
        }

        // GET: api/TypesOfWork/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeOfWork>> GetTypeOfWork(int id)
        {
            if (_context.TypesOfWork == null)
            {
                return NotFound();
            }
            var tp = await _context.TypesOfWork.FindAsync(id);

            if (tp == null)
            {
                return NotFound();
            }

            return tp;
        }

        // PUT: api/TypesOfWork/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> PutTypeOfWork(int id, TypeOfWork tp)
        {
            if (id != tp.Id)
            {
                return BadRequest();
            }

            _context.Entry(tp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeOfWorkExists(id))
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

        // POST: api/TypesOfWork
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<TypeOfWork>> PostTypeOfWork(TypeOfWorkDTO tpDTO)
        {
            if (_context.TypesOfWork == null)
            {
                return Problem("Entity set 'OrderContext.TypesOfWork'  is null.");
            }
            TypeOfWork tp = new TypeOfWork(tpDTO);
            _context.TypesOfWork.Add(tp);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        // DELETE: api/TypesOfWork/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteTypeOfWork(int id)
        {
            if (_context.TypesOfWork == null)
            {
                return NotFound();
            }
            var tp = await _context.TypesOfWork.FindAsync(id);
            if (tp == null)
            {
                return NotFound();
            }

            _context.TypesOfWork.Remove(tp);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TypeOfWorkExists(int id)
        {
            return (_context.TypesOfWork?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///

        [HttpGet("search/{name}")]
        public async Task<ActionResult<IEnumerable<TypeOfWorkOutDTO>>> SearchTypeOfWork(string name)
        {
            if (_context.TypesOfWork == null)
            {
                return NotFound();
            }
            //var tp = _context.TypesOfWork.Where(t => t.Name.StartsWith(name)).First();
            List<TypeOfWorkOutDTO> types = (from o in _context.TypesOfWork
                                            where o.Name.StartsWith(name)
                                            select new TypeOfWorkOutDTO()
                                            {
                                                Id = o.Id,
                                                Name = o.Name,
                                                Description = o.Description,
                                                Duration = o.Duration,
                                                Cost = o.Cost
                                            } ).ToList();
            if (types == null)
            {
                return NotFound();
            }
            return types;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("/api/Types/json/")]
        public async Task<ActionResult<IEnumerable<TypeOfWorkOutDTO>>> GetOrdersForFront()
        {
            if (_context.TypesOfWork == null)
            {
                return NotFound();
            }
            //var orders = await _context.Orders.Include(o => o.TypeOfWork).Include(o => o.Client).Select(new OrderOutDTO() { Id = })
            List<TypeOfWorkOutDTO> types = (from o in _context.TypesOfWork
                                            select new TypeOfWorkOutDTO()
                                            {
                                                Id = o.Id,
                                                Name = o.Name,
                                                Description = o.Description,
                                                Duration = o.Duration,
                                                Cost = o.Cost
                                            }).ToList();

            return types;
        }

    }
}
