using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public KhachHangController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhachHang>>> GetAll()
        {
            return await _context.KhachHang
                .Include(k => k.DonHang)
                .OrderByDescending(k => k.MaKhachHang)
                .ToListAsync();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<KhachHang>>> Search([FromQuery] string? keyword)
        {
            var query = _context.KhachHang.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(k => k.HoTen.Contains(keyword) || 
                                       k.SoDienThoai.Contains(keyword) ||
                                       k.Email.Contains(keyword));
            }

            return await query
                .Include(k => k.DonHang)
                .OrderByDescending(k => k.MaKhachHang)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<KhachHang>> Create(KhachHangDTO dto)
        {
            if (await _context.KhachHang.AnyAsync(k => k.SoDienThoai == dto.SoDienThoai))
                return BadRequest("Số điện thoại đã tồn tại");

            var khachHang = new KhachHang
            {
                HoTen = dto.HoTen,
                SoDienThoai = dto.SoDienThoai,
                Email = dto.Email
            };

            _context.KhachHang.Add(khachHang);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = khachHang.MaKhachHang }, khachHang);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, KhachHangDTO dto)
        {
            var khachHang = await _context.KhachHang.FindAsync(id);
            if (khachHang == null) return NotFound();

            // Kiểm tra trùng SĐT khi cập nhật
            if (await _context.KhachHang.AnyAsync(k => 
                k.MaKhachHang != id && k.SoDienThoai == dto.SoDienThoai))
                return BadRequest("Số điện thoại đã tồn tại");

            khachHang.HoTen = dto.HoTen;
            khachHang.SoDienThoai = dto.SoDienThoai;
            khachHang.Email = dto.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("lichsu/{id}")]
        public async Task<ActionResult<object>> GetLichSu(int id)
        {
            var khachHang = await _context.KhachHang
                .Include(k => k.DonHang)
                .FirstOrDefaultAsync(k => k.MaKhachHang == id);

            if (khachHang == null) return NotFound();

            return Ok(new
            {
                KhachHang = khachHang,
                TongChiTieu = khachHang.DonHang?.Sum(d => d.TongTien) ?? 0,
                SoDonHang = khachHang.DonHang?.Count ?? 0
            });
        }
    }
}
