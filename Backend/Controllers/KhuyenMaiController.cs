using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public KhuyenMaiController(RestaurantDbContext context)
        {
            _context = context;
        }

        // DTO cho request thêm/sửa khuyến mãi
        public class KhuyenMaiRequest
        {
            public string TenKhuyenMai { get; set; }
            public string MoTa { get; set; }
            public decimal PhanTramGiam { get; set; }
            public DateTime NgayBatDau { get; set; }
            public DateTime NgayKetThuc { get; set; }
            public string DieuKien { get; set; }
            public bool TrangThai { get; set; }
            public List<int> MaLoaiMon { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetAll()
        {
            var khuyenMais = await _context.KhuyenMai
                .Include(k => k.KhuyenMai_MonAn)
                    .ThenInclude(km => km.MonAn)
                        .ThenInclude(m => m.LoaiMon)
                .Select(k => new
                {
                    k.MaKhuyenMai,
                    k.TenKhuyenMai,
                    k.MoTa,
                    k.PhanTramGiam,
                    k.NgayBatDau,
                    k.NgayKetThuc,
                    k.DieuKien,
                    k.TrangThai,
                    MaLoaiMon = k.KhuyenMai_MonAn
                        .Select(km => km.MonAn.LoaiMon.MaLoai)
                        .Distinct()
                        .ToList()
                })
                .OrderByDescending(k => k.NgayBatDau)
                .ToListAsync();

            return Ok(khuyenMais);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<KhuyenMai>>> GetActive()
        {
            var now = DateTime.UtcNow;
            return await _context.KhuyenMai
                .Where(k => k.NgayBatDau <= now && k.NgayKetThuc >= now)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<KhuyenMai>> Create(KhuyenMaiRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Tạo khuyến mãi mới
                var khuyenMai = new KhuyenMai
                {
                    TenKhuyenMai = request.TenKhuyenMai,
                    MoTa = request.MoTa,
                    PhanTramGiam = request.PhanTramGiam,
                    NgayBatDau = request.NgayBatDau,
                    NgayKetThuc = request.NgayKetThuc,
                    DieuKien = request.DieuKien,
                    TrangThai = request.TrangThai
                };

                _context.KhuyenMai.Add(khuyenMai);
                await _context.SaveChangesAsync();

                // Lấy danh sách món ăn thuộc các loại món được chọn
                var monAns = await _context.MonAn
                    .Where(m => request.MaLoaiMon.Contains(m.MaLoai))
                    .ToListAsync();

                // Thêm quan hệ khuyến mãi - món ăn
                foreach (var monAn in monAns)
                {
                    _context.KhuyenMai_MonAn.Add(new KhuyenMai_MonAn
                    {
                        MaKhuyenMai = khuyenMai.MaKhuyenMai,
                        MaMon = monAn.MaMon
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetAll), new { id = khuyenMai.MaKhuyenMai }, khuyenMai);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, KhuyenMaiRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var khuyenMai = await _context.KhuyenMai
                    .Include(k => k.KhuyenMai_MonAn)
                    .FirstOrDefaultAsync(k => k.MaKhuyenMai == id);

                if (khuyenMai == null) return NotFound();

                // Cập nhật thông tin khuyến mãi
                khuyenMai.TenKhuyenMai = request.TenKhuyenMai;
                khuyenMai.MoTa = request.MoTa;
                khuyenMai.PhanTramGiam = request.PhanTramGiam;
                khuyenMai.NgayBatDau = request.NgayBatDau;
                khuyenMai.NgayKetThuc = request.NgayKetThuc;
                khuyenMai.DieuKien = request.DieuKien;
                khuyenMai.TrangThai = request.TrangThai;

                // Xóa các quan hệ cũ
                _context.KhuyenMai_MonAn.RemoveRange(khuyenMai.KhuyenMai_MonAn);

                // Lấy danh sách món ăn mới
                var monAns = await _context.MonAn
                    .Where(m => request.MaLoaiMon.Contains(m.MaLoai))
                    .ToListAsync();

                // Thêm quan hệ mới
                foreach (var monAn in monAns)
                {
                    _context.KhuyenMai_MonAn.Add(new KhuyenMai_MonAn
                    {
                        MaKhuyenMai = khuyenMai.MaKhuyenMai,
                        MaMon = monAn.MaMon
                    });
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var khuyenMai = await _context.KhuyenMai
                .Include(k => k.KhuyenMai_MonAn)
                .FirstOrDefaultAsync(k => k.MaKhuyenMai == id);

            if (khuyenMai == null) return NotFound();

            _context.KhuyenMai_MonAn.RemoveRange(khuyenMai.KhuyenMai_MonAn);
            _context.KhuyenMai.Remove(khuyenMai);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("stats/{id}")]
        public async Task<ActionResult<dynamic>> GetStats(int id)
        {
            var stats = await _context.LichSuKhuyenMai
                .Where(l => l.MaKhuyenMai == id)
                .GroupBy(l => l.MaKhuyenMai)
                .Select(g => new
                {
                    SoDonHang = g.Count(),
                    TongDoanhThu = g.Sum(l => l.DonHang.TongTien),
                    TongGiamGia = g.Sum(l => l.SoTienGiam)
                })
                .FirstOrDefaultAsync();

            return Ok(stats ?? new { SoDonHang = 0, TongDoanhThu = 0m, TongGiamGia = 0m });
        }
    }
}
