using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChamCongController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public ChamCongController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet("nhanvien/{maNV}")]
        public async Task<ActionResult<IEnumerable<ChamCong>>> GetByNhanVien(
            int maNV, [FromQuery] DateTime? tuNgay, [FromQuery] DateTime? denNgay)
        {
            var query = _context.ChamCong.Where(c => c.MaNhanVien == maNV);

            if (tuNgay.HasValue)
                query = query.Where(c => c.NgayChamCong >= tuNgay);
            if (denNgay.HasValue)
                query = query.Where(c => c.NgayChamCong <= denNgay);

            return await query.OrderByDescending(c => c.NgayChamCong).ToListAsync();
        }

        [HttpPost("checkin")]
        public async Task<ActionResult<ChamCong>> CheckIn(int maNV)
        {
            var today = DateTime.UtcNow.Date;
            var existingCheckin = await _context.ChamCong
                .FirstOrDefaultAsync(c => c.MaNhanVien == maNV && 
                                        c.NgayChamCong.Date == today);

            if (existingCheckin != null)
                return BadRequest("Đã chấm công ngày hôm nay");

            var chamCong = new ChamCong
            {
                MaNhanVien = maNV,
                NgayChamCong = DateTime.UtcNow,
                GioVao = DateTime.UtcNow.TimeOfDay,
                TrangThai = "Đang làm việc"
            };

            _context.ChamCong.Add(chamCong);
            await _context.SaveChangesAsync();

            return Ok(chamCong);
        }

        [HttpPut("checkout/{id}")]
        public async Task<IActionResult> CheckOut(int id)
        {
            var chamCong = await _context.ChamCong.FindAsync(id);
            if (chamCong == null) return NotFound();

            chamCong.GioRa = DateTime.UtcNow.TimeOfDay;
            chamCong.TrangThai = "Đã về";
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
