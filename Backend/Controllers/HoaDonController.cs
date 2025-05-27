using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Data;
using Backend.DTOs;


namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public HoaDonController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: api/HoaDon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetHoaDon()
        {
            return await _context.HoaDon
                .Include(h => h.DonHang)
                    .ThenInclude(d => d.KhachHang)
                .Include(h => h.NhanVien)
                .ToListAsync();
        }

        // GET: api/HoaDon/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HoaDon>> GetHoaDon(int id)
        {
            var hoaDon = await _context.HoaDon
                .Include(h => h.DonHang)
                    .ThenInclude(d => d.KhachHang)
                .Include(h => h.NhanVien)
                .FirstOrDefaultAsync(h => h.MaHoaDon == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            return hoaDon;
        }

        // POST: api/HoaDon/TaoTuDonHang/5
        [HttpPost("TaoTuDonHang/{maDonHang}")]
        public async Task<ActionResult<HoaDon>> TaoHoaDonTuDonHang(int maDonHang, [FromBody] ThanhToanDTO thanhToanDTO)
        {
            var donHang = await _context.DonHang
                .Include(d => d.ChiTietDonHang)
                .FirstOrDefaultAsync(d => d.MaDonHang == maDonHang);

            if (donHang == null)
            {
                return NotFound("Không tìm thấy đơn hàng");
            }

            if (donHang.TrangThai != "HoanThanh")
            {
                return BadRequest("Đơn hàng chưa hoàn thành");
            }

            var hoaDon = new HoaDon
            {
                MaDonHang = maDonHang,
                ThoiGianThanhToan = DateTime.Now,
                TongTienHang = donHang.TongTien,
                GiamGia = thanhToanDTO.GiamGia,
                TongThanhToan = donHang.TongTien - thanhToanDTO.GiamGia,
                PhuongThucThanhToan = thanhToanDTO.PhuongThucThanhToan,
                MaNhanVien = thanhToanDTO.MaNhanVien,
                GhiChu = thanhToanDTO.GhiChu
            };

            _context.HoaDon.Add(hoaDon);
            donHang.TrangThai = "DaThanhToan";

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHoaDon), new { id = hoaDon.MaHoaDon }, hoaDon);
        }

        // PUT: api/HoaDon/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHoaDon(int id, HoaDon hoaDon)
        {
            if (id != hoaDon.MaHoaDon)
            {
                return BadRequest();
            }

            _context.Entry(hoaDon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoaDonExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        private bool HoaDonExists(int id)
        {
            return _context.HoaDon.Any(e => e.MaHoaDon == id);
        }
    }
}