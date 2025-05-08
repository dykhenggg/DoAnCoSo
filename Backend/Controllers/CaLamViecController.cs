using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaLamViecController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        public CaLamViecController(RestaurantDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaLamViec>>> Get() => 
            await _context.CaLamViec
                .Include(c => c.NhanVien)
                .ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<CaLamViec>> Get(int id)
        {
            var caLamViec = await _context.CaLamViec
                .Include(c => c.NhanVien)
                .FirstOrDefaultAsync(c => c.MaCa == id);
            return caLamViec == null ? NotFound() : caLamViec;
        }

        [HttpPost]
        public async Task<ActionResult<CaLamViec>> Post(CaLamViec caLamViec)
        {
            _context.CaLamViec.Add(caLamViec);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = caLamViec.MaCa }, caLamViec);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CaLamViec caLamViec)
        {
            if (id != caLamViec.MaCa) return BadRequest();
            _context.Entry(caLamViec).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var caLamViec = await _context.CaLamViec.FindAsync(id);
            if (caLamViec == null) return NotFound();
            _context.CaLamViec.Remove(caLamViec);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("by-shift")]
        public async Task<ActionResult<IEnumerable<CaLamViec>>> GetByShift(string shift)
        {
            return await _context.CaLamViec
                .Include(c => c.NhanVien)
                .ToListAsync();
        }
    }
}