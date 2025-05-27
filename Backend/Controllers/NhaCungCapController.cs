using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhaCungCapController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public NhaCungCapController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: api/NhaCungCap
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhaCungCap>>> GetNhaCungCap()
        {
            return await _context.NhaCungCap.ToListAsync();
        }

        // GET: api/NhaCungCap/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NhaCungCap>> GetNhaCungCap(int id)
        {
            var nhaCungCap = await _context.NhaCungCap.FindAsync(id);

            if (nhaCungCap == null)
            {
                return NotFound();
            }

            return nhaCungCap;
        }

        // PUT: api/NhaCungCap/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhaCungCap(int id, NhaCungCap nhaCungCap)
        {
            if (id != nhaCungCap.MaNCC)
            {
                return BadRequest();
            }

            _context.Entry(nhaCungCap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhaCungCapExists(id))
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

        // POST: api/NhaCungCap
        [HttpPost]
        public async Task<ActionResult<NhaCungCap>> PostNhaCungCap(NhaCungCap nhaCungCap)
        {
            _context.NhaCungCap.Add(nhaCungCap);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNhaCungCap", new { id = nhaCungCap.MaNCC }, nhaCungCap);
        }

        // DELETE: api/NhaCungCap/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhaCungCap(int id)
        {
            var nhaCungCap = await _context.NhaCungCap.FindAsync(id);
            if (nhaCungCap == null)
            {
                return NotFound();
            }

            _context.NhaCungCap.Remove(nhaCungCap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NhaCungCapExists(int id)
        {
            return _context.NhaCungCap.Any(e => e.MaNCC == id);
        }
    }
}