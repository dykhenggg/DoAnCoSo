using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonHangController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        public DonHangController(RestaurantDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonHang>>> Get() => 
            await _context.DonHang
                .Include(d => d.KhachHang)
                .Include(d => d.ChiTietDonHang)
                .ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<DonHang>> Get(int id)
        {
            var donHang = await _context.DonHang
                .Include(d => d.KhachHang)
                .Include(d => d.ChiTietDonHang)
                .FirstOrDefaultAsync(d => d.MaDonHang == id);
            return donHang == null ? NotFound() : donHang;
        }

        [HttpPost]
        public async Task<ActionResult<DonHang>> Post(DonHang donHang)
        {
            _context.DonHang.Add(donHang);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = donHang.MaDonHang }, donHang);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, DonHang donHang)
        {
            if (id != donHang.MaDonHang) return BadRequest();
            _context.Entry(donHang).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var donHang = await _context.DonHang.FindAsync(id);
            if (donHang == null) return NotFound();
            _context.DonHang.Remove(donHang);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("by-status")]
        public async Task<ActionResult<IEnumerable<DonHang>>> GetByStatus(string status)
        {
            return await _context.DonHang
                .Include(d => d.KhachHang)
                .Where(d => d.TrangThai == status)
                .ToListAsync();
        }

        [HttpGet("by-date-range")]
        public async Task<ActionResult<IEnumerable<DonHang>>> GetByDateRange(
            DateTime startDate, 
            DateTime endDate)
        {
            return await _context.DonHang
                .Include(d => d.KhachHang)
                .Where(d => d.NgayDat >= startDate && d.NgayDat <= endDate)
                .ToListAsync();
        }
    }
}