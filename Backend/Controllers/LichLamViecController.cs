using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

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

        [HttpGet("tuan")]
        public async Task<ActionResult<IEnumerable<LichLamViec>>> GetLichTuan([FromQuery] DateTime startDate)
        {
            var endDate = startDate.AddDays(7);
            return await _context.LichLamViec
                .Include(l => l.NhanVien)
                .Include(l => l.CaLamViec)
                .Where(l => l.NgayLamViec >= startDate && l.NgayLamViec < endDate)
                .OrderBy(l => l.NgayLamViec)
                .ThenBy(l => l.CaLamViec.GioBatDau)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<LichLamViec>> Create(LichLamViecDTO dto)
        {
            var exists = await _context.LichLamViec
                .AnyAsync(l => l.MaNhanVien == dto.MaNV && 
                              l.NgayLamViec.Date == dto.NgayLamViec.Date &&
                              l.MaCa == dto.MaCa);

            if (exists)
                return BadRequest("Đã có lịch làm việc trong thời gian này");

            var lichLamViec = new LichLamViec
            {
                MaNhanVien = dto.MaNV,
                MaCa = dto.MaCa,
                NgayLamViec = dto.NgayLamViec,
                GhiChu = dto.GhiChu ?? string.Empty
            };

            _context.LichLamViec.Add(lichLamViec);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLichTuan), new { id = lichLamViec.MaLichLamViec }, lichLamViec);
        }
    }
}
