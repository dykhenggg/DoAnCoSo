using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;
using System.Linq;
using System.Globalization;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public BanController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

            var tables = await _context.Ban
                .Include(b => b.DatBan)
                .ToListAsync();

            var result = tables.Select(b => new {
                b.MaBan,
                b.TenBan,
                b.SucChua,
                TrangThai = !b.DatBan.Any(d => {
                    try {
                        if (d.ThoiGianBatDau == null || d.ThoiGianKetThuc == null) return false;
                        var start = TimeZoneInfo.ConvertTimeFromUtc(d.ThoiGianBatDau, tz);
                        var end = TimeZoneInfo.ConvertTimeFromUtc(d.ThoiGianKetThuc, tz);
                        return start <= now && end > now;
                    } catch { return false; }
                }),
                b.DatBan
            }).ToList();

            return Ok(result);
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Ban>>> GetAvailable(
            [FromQuery] int soNguoi,
            [FromQuery] DateTime thoiGianBatDau,
            [FromQuery] DateTime thoiGianKetThuc)
        {
            return await _context.Ban
                .Where(b => b.SucChua >= soNguoi &&
                           !b.DatBan.Any(d => d.ThoiGianBatDau < thoiGianKetThuc &&
                                             d.ThoiGianKetThuc > thoiGianBatDau))
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Ban>> Create(BanDTO dto)
        {
            var ban = new Ban
            {
                TenBan = dto.TenBan,
                SucChua = dto.SucChua,
                TrangThai = true
            };

            _context.Ban.Add(ban);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = ban.MaBan }, ban);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BanDTO dto)
        {
            var ban = await _context.Ban.FindAsync(id);
            if (ban == null) return NotFound();

            // Kiểm tra tên bàn trùng
            var existingBan = await _context.Ban
                .FirstOrDefaultAsync(b => b.TenBan == dto.TenBan && b.MaBan != id);
            if (existingBan != null)
                return BadRequest("Tên bàn đã tồn tại");

            ban.TenBan = dto.TenBan;
            ban.SucChua = dto.SucChua;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}/trangthai")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] bool trangThai)
        {
            var ban = await _context.Ban.FindAsync(id);
            if (ban == null) return NotFound();

            ban.TrangThai = trangThai;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("update-all-status")]
        public async Task<IActionResult> UpdateAllStatus([FromBody] bool trangThai)
        {
            var allTables = await _context.Ban.ToListAsync();
            foreach (var ban in allTables)
            {
                ban.TrangThai = trangThai;
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ban = await _context.Ban
                .Include(b => b.DatBan)
                .FirstOrDefaultAsync(b => b.MaBan == id);

            if (ban == null) return NotFound();

            // Kiểm tra xem bàn có đang được đặt không
            var coLichDat = ban.DatBan.Any(d => 
                d.ThoiGianBatDau <= DateTime.UtcNow && 
                d.ThoiGianKetThuc >= DateTime.UtcNow);

            if (coLichDat)
                return BadRequest("Không thể xóa bàn đang được đặt");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Ban.Remove(ban);
                await _context.SaveChangesAsync();

                // Lấy danh sách các bàn còn lại và sắp xếp theo mã bàn
                var remainingTables = await _context.Ban
                    .OrderBy(b => b.MaBan)
                    .ToListAsync();

                // Cập nhật lại mã bàn
                for (int i = 0; i < remainingTables.Count; i++)
                {
                    remainingTables[i].MaBan = i + 1;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return NoContent();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
