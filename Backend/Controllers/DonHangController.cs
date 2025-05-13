using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

[Route("api/[controller]")]
[ApiController]
public class DonHangController : ControllerBase
{
    private readonly RestaurantDbContext _context;

    public DonHangController(RestaurantDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<DonHang>> CreateDonHang(DonHangDTO dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var donHang = new DonHang
            {
                KhachHangID = dto.MaKhachHang,
                NgayDat = DateTime.UtcNow,
                TrangThai = "Đang xử lý"
            };

            _context.DonHang.Add(donHang);
            await _context.SaveChangesAsync();

            decimal tongTien = 0;
            foreach (var item in dto.ChiTietList)
            {
                var thucDon = await _context.ThucDon.FindAsync(item.MaMon);
                if (thucDon == null) continue;

                var chiTiet = new ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaMon = item.MaMon,
                    SoLuong = item.SoLuong,
                    DonGia = thucDon.Gia,
                    ThanhTien = thucDon.Gia * item.SoLuong
                };
                tongTien += chiTiet.ThanhTien;
                _context.ChiTietDonHang.Add(chiTiet);
            }

            donHang.TongTien = tongTien;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return CreatedAtAction(nameof(GetById), new { id = donHang.MaDonHang }, donHang);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DonHang>> GetById(int id)
    {
        var donHang = await _context.DonHang
            .Include(d => d.KhachHang)
            .Include(d => d.ChiTietDonHang)
                .ThenInclude(c => c.ThucDon)
            .FirstOrDefaultAsync(d => d.MaDonHang == id);

        return donHang == null ? NotFound() : donHang;
    }

    [HttpPut("{id}/trangthai")]
    public async Task<IActionResult> UpdateTrangThai(int id, [FromBody] string trangThai)
    {
        var donHang = await _context.DonHang.FindAsync(id);
        if (donHang == null) return NotFound();

        donHang.TrangThai = trangThai;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
