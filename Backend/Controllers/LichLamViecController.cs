using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LichLamViecController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public LichLamViecController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LichLamViec>>> GetLichLamViec()
        {
            return await _context.LichLamViec
                .Include(l => l.NhanVien)
                .Include(l => l.CaLamViec)
                .ToListAsync();
        }

        [HttpGet("nhanvien/{maNV}")]
        public async Task<ActionResult<IEnumerable<LichLamViec>>> GetLichLamViecByNhanVien(int maNV)
        {
            return await _context.LichLamViec
                .Include(l => l.CaLamViec)
                .Include(l => l.NhanVien)
                .Where(l => l.MaNV == maNV)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<LichLamViec>> CreateLichLamViec(LichLamViec lichLamViec)
        {
            _context.LichLamViec.Add(lichLamViec);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLichLamViec), new { id = lichLamViec.MaLichLamViec }, lichLamViec);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLichLamViec(int id)
        {
            var lichLamViec = await _context.LichLamViec.FindAsync(id);
            if (lichLamViec == null) return NotFound();

            _context.LichLamViec.Remove(lichLamViec);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
