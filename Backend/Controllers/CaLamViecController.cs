using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaLamViecController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public CaLamViecController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaLamViec>>> GetAll()
        {
            return await _context.CaLamViec
                .Include(c => c.NhanVien)
                .ToListAsync();
        }

        [HttpGet("nhanvien/{maNV}")]
        public async Task<ActionResult<IEnumerable<CaLamViec>>> GetByNhanVien(int maNV)
        {
            return await _context.CaLamViec
                .Where(c => c.MaNhanVien == maNV)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<CaLamViec>> Create(CaLamViecDTO dto)
        {
            var caLamViec = new CaLamViec
            {
                MaNhanVien = dto.MaNhanVien,
                GioBatDau = dto.GioBatDau,
                GioKetThuc = dto.GioKetThuc
            };

            _context.CaLamViec.Add(caLamViec);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = caLamViec.MaCa }, caLamViec);
        }
    }
}
