using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LichLamViecController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        public LichLamViecController(RestaurantDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LichLamViec>>> Get() => 
            await _context.LichLamViec
                .Include(l => l.NhanVien)
                .Include(l => l.CaLamViec)
                .ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<LichLamViec>> Get(int id)
        {
            var lichLamViec = await _context.LichLamViec
                .Include(l => l.NhanVien)
                .Include(l => l.CaLamViec)
                .FirstOrDefaultAsync(l => l.MaLichLamViec == id);
            return lichLamViec == null ? NotFound() : lichLamViec;
        }

        [HttpPost]
        public async Task<ActionResult<LichLamViec>> Post(LichLamViec lichLamViec)
        {
            _context.LichLamViec.Add(lichLamViec);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = lichLamViec.MaLichLamViec }, lichLamViec);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, LichLamViec lichLamViec)
        {
            if (id != lichLamViec.MaLichLamViec) return BadRequest();
            _context.Entry(lichLamViec).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lichLamViec = await _context.LichLamViec.FindAsync(id);
            if (lichLamViec == null) return NotFound();
            _context.LichLamViec.Remove(lichLamViec);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("by-date")]
        public async Task<ActionResult<IEnumerable<LichLamViec>>> GetByDate(DateTime date)
        {
            return await _context.LichLamViec
                .Include(l => l.NhanVien)
                .Include(l => l.CaLamViec)
                .Where(l => l.NgayLamViec.Date == date.Date)
                .ToListAsync();
        }

        [HttpGet("by-employee")]
        public async Task<ActionResult<IEnumerable<LichLamViec>>> GetByEmployee(int employeeId)
        {
            return await _context.LichLamViec
                .Include(l => l.CaLamViec)
                .Where(l => l.MaNhanVien == employeeId)
                .ToListAsync();
        }
    }
}