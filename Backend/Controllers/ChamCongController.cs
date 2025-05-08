using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChamCongController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        public ChamCongController(RestaurantDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChamCong>>> Get() => 
            await _context.ChamCong
                .Include(c => c.NhanVien)
                .ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<ChamCong>> Get(int id)
        {
            var chamCong = await _context.ChamCong
                .Include(c => c.NhanVien)
                .FirstOrDefaultAsync(c => c.MaChamCong == id);
            return chamCong == null ? NotFound() : chamCong;
        }

        [HttpPost]
        public async Task<ActionResult<ChamCong>> Post(ChamCong chamCong)
        {
            _context.ChamCong.Add(chamCong);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = chamCong.MaChamCong }, chamCong);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ChamCong chamCong)
        {
            if (id != chamCong.MaChamCong) return BadRequest();
            _context.Entry(chamCong).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var chamCong = await _context.ChamCong.FindAsync(id);
            if (chamCong == null) return NotFound();
            _context.ChamCong.Remove(chamCong);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("by-date-range")]
        public async Task<ActionResult<IEnumerable<ChamCong>>> GetByDateRange(
            DateTime startDate, 
            DateTime endDate)
        {
            return await _context.ChamCong
                .Include(c => c.NhanVien)
                .Where(c => c.NgayChamCong >= startDate && c.NgayChamCong <= endDate)
                .ToListAsync();
        }

        [HttpGet("by-employee")]
        public async Task<ActionResult<IEnumerable<ChamCong>>> GetByEmployee(int employeeId)
        {
            return await _context.ChamCong
                .Where(c => c.MaNhanVien == employeeId)
                .ToListAsync();
        }
    }
}