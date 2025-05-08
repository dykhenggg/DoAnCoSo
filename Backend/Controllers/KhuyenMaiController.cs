using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KhuyenMaiController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        public KhuyenMaiController(RestaurantDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhuyenMai>>> Get() => 
            await _context.KhuyenMai.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<KhuyenMai>> Get(int id)
        {
            var khuyenMai = await _context.KhuyenMai.FindAsync(id);
            return khuyenMai == null ? NotFound() : khuyenMai;
        }

        [HttpPost]
        public async Task<ActionResult<KhuyenMai>> Post(KhuyenMai khuyenMai)
        {
            _context.KhuyenMai.Add(khuyenMai);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = khuyenMai.MaKM }, khuyenMai);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, KhuyenMai khuyenMai)
        {
            if (id != khuyenMai.MaKM) return BadRequest();
            _context.Entry(khuyenMai).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var khuyenMai = await _context.KhuyenMai.FindAsync(id);
            if (khuyenMai == null) return NotFound();
            _context.KhuyenMai.Remove(khuyenMai);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<KhuyenMai>>> GetActive()
        {
            var currentDate = DateTime.Now;
            return await _context.KhuyenMai
                .Where(k => k.NgayBatDau <= currentDate && k.NgayKetThuc >= currentDate)
                .ToListAsync();
        }

        [HttpGet("by-date-range")]
        public async Task<ActionResult<IEnumerable<KhuyenMai>>> GetByDateRange(
            DateTime startDate, 
            DateTime endDate)
        {
            return await _context.KhuyenMai
                .Where(k => k.NgayBatDau >= startDate && k.NgayKetThuc <= endDate)
                .ToListAsync();
        }
    }
}