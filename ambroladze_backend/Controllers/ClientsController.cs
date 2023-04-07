using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ambroladze_backend.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using ambroladze_backend.DTO;

namespace ambroladze_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly OrderContext _context;

        public ClientsController(OrderContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetUsers()
        {
          if (_context.Clients == null)
          {
              return NotFound();
          }
            return await _context.Clients.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetUser(int id)
        {
          if (_context.Clients == null)
          {
              return NotFound();
          }
            var user = await _context.Clients.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, Client user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Client>> PostUser(ClientDTO userDTO)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'Context.Users' is null.");
            }
            Client user = new Client(userDTO);
            if (user == null)
            {
                return Problem("Error");
            }
            if (user.Login.StartsWith("worker_") && (!User.IsInRole("admin")))
            {
                return Problem("Workers can only be added by the admin.");
            }

            _context.Clients.Add(user);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }
            var user = await _context.Clients.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
