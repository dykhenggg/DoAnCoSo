using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;
using Microsoft.AspNetCore.Authorization;

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
                .Include(d => d.DatBanMonAn)
                    .ThenInclude(dm => dm.MonAn)
                .OrderByDescending(d => d.NgayDat)
                .ToListAsync();
        }

        [AllowAnonymous]
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

                // Lưu thông tin món ăn nếu có
                if (dto.MonAn != null && dto.MonAn.Any())
                {
                    foreach (var monAn in dto.MonAn)
                    {
                        var monAnEntity = await _context.MonAn.FindAsync(monAn.MaMon);
                        if (monAnEntity == null)
                            return BadRequest($"Không tìm thấy món ăn với mã {monAn.MaMon}");

                        var datBanMonAn = new DatBanMonAn
                        {
                            MaDatBan = datBan.MaDatBan,
                            MaMon = monAn.MaMon,
                            SoLuong = monAn.SoLuong,
                            DonGia = monAnEntity.Gia,
                            GhiChu = monAn.GhiChu
                        };

                        _context.DatBanMonAn.Add(datBanMonAn);
                    }
                    await _context.SaveChangesAsync();
                }

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
            var datBan = await _context.DatBan
                .Include(d => d.DatBanMonAn)
                .FirstOrDefaultAsync(d => d.MaDatBan == id);

            if (datBan == null) return NotFound();

            if (datBan.ThoiGianBatDau <= DateTime.UtcNow)
                return BadRequest("Không thể hủy đặt bàn đã bắt đầu");

            // Xóa các món ăn liên quan
            _context.DatBanMonAn.RemoveRange(datBan.DatBanMonAn);
            _context.DatBan.Remove(datBan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{maDatBan}/MonAn")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetMonAnByDatBan(int maDatBan)
        {
            try
            {
                var monAns = await _context.DatBanMonAn
                    .Include(d => d.MonAn)
                    .Where(d => d.MaDatBan == maDatBan)
                    .Select(d => new
                    {
                        d.Id,
                        d.MaDatBan,
                        d.MaMon,
                        d.SoLuong,
                        d.DonGia,
                        d.GhiChu,
                        TenMon = d.MonAn.TenMon
                    })
                    .ToListAsync();

                if (monAns == null || !monAns.Any())
                {
                    return NotFound(new { message = "Không tìm thấy món ăn cho đặt bàn này" });
                }

                return Ok(monAns);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy thông tin món ăn", error = ex.Message });
            }
        }
    }
}
