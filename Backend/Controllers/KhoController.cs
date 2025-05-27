using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhoController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public KhoController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet("canbaotoncao")]
        public async Task<ActionResult<IEnumerable<Kho>>> GetSapHet()
        {
            return await _context.Kho
                .Where(k => k.SoLuongHienTai <= k.SoLuongToiThieu)
                .OrderBy(k => k.SoLuongHienTai)
                .ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kho>>> GetAll()
        {
            return await _context.Kho
                .OrderBy(k => k.TenNguyenLieu)
                .ToListAsync();
        }

        [HttpPost("nhapkho")]
        public async Task<ActionResult> NhapKho(GiaoDichKhoDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var nguyenLieu = await _context.Kho.FindAsync(dto.MaNguyenLieu);
                if (nguyenLieu == null) return NotFound();

                nguyenLieu.SoLuongHienTai += dto.SoLuong;
                
                var giaoDich = new GiaoDichKho
                {
                    MaNguyenLieu = dto.MaNguyenLieu,
                    SoLuong = dto.SoLuong,
                    Loai = "NhapKho",
                    NgayGio = DateTime.UtcNow,
                    LyDo = dto.GhiChu ?? string.Empty
                };

                _context.GiaoDichKho.Add(giaoDich);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(giaoDich);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
