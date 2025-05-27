using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatBanController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public DatBanController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DatBan>>> GetAll()
        {
            return await _context.DatBan
                .Include(d => d.Ban)
                .Include(d => d.KhachHang)
                .Select(d => new {
                    d.MaDatBan,
                    d.NgayDat,
                    d.ThoiGianBatDau,
                    d.ThoiGianKetThuc,
                    d.SoNguoi,
                    d.GhiChu,
                    TenKhachHang = d.KhachHang.HoTen,
                    SoDienThoai = d.KhachHang.SoDienThoai,
                    TenBan = d.Ban.TenBan
                })
                .OrderByDescending(d => d.NgayDat)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<DatBan>> Create(DatBanDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Kiểm tra trùng lịch
                var coTrungLich = await _context.DatBan
                    .AnyAsync(d => d.MaBan == dto.MaBan &&
                                  d.ThoiGianBatDau < dto.ThoiGianKetThuc &&
                                  d.ThoiGianKetThuc > dto.ThoiGianBatDau);

                if (coTrungLich)
                    return BadRequest("Bàn đã được đặt trong thời gian này");

                var datBan = new DatBan
                {
                    MaBan = dto.MaBan,
                    MaKH = dto.MaKH,
                    NgayDat = DateTime.UtcNow,
                    ThoiGianBatDau = dto.ThoiGianBatDau,
                    ThoiGianKetThuc = dto.ThoiGianKetThuc,
                    SoNguoi = dto.SoNguoi,
                    GhiChu = dto.GhiChu
                };

                _context.DatBan.Add(datBan);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetAll), new { id = datBan.MaDatBan }, datBan);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var datBan = await _context.DatBan.FindAsync(id);
            if (datBan == null) return NotFound();

            if (datBan.ThoiGianBatDau <= DateTime.UtcNow)
                return BadRequest("Không thể hủy đặt bàn đã bắt đầu");

            _context.DatBan.Remove(datBan);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
