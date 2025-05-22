using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoPhanController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public BoPhanController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoPhanDTO>>> GetAll()
        {
            var boPhans = await _context.BoPhan
                .Include(b => b.NhanVien)
                .ToListAsync();

            var boPhanDTOs = boPhans.Select(b => new BoPhanDTO
            {
                MaBoPhan = b.MaBoPhan,
                TenBoPhan = b.TenBoPhan,
                MoTa = b.MoTa,
                SoLuongNhanVien = b.NhanVien.Count
            });

            return Ok(boPhanDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BoPhan>> GetById(int id)
        {
            var boPhan = await _context.BoPhan
                .Include(b => b.NhanVien)
                .FirstOrDefaultAsync(b => b.MaBoPhan == id);

            if (boPhan == null) return NotFound();

            return boPhan;
        }

        [HttpPost]
        public async Task<ActionResult<BoPhan>> Create(BoPhan boPhan)
        {
            try
            {
                // Kiểm tra tên bộ phận trống
                if (string.IsNullOrWhiteSpace(boPhan.TenBoPhan))
                    return BadRequest("Tên bộ phận không được để trống");

                // Kiểm tra tên bộ phận đã tồn tại
                var exists = await _context.BoPhan
                    .AnyAsync(b => b.TenBoPhan.ToLower() == boPhan.TenBoPhan.ToLower());
                if (exists)
                    return BadRequest("Tên bộ phận đã tồn tại");

                _context.BoPhan.Add(boPhan);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = boPhan.MaBoPhan }, boPhan);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi thêm bộ phận: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BoPhan boPhan)
        {
            if (id != boPhan.MaBoPhan) return BadRequest();

            try
            {
                // Kiểm tra tên bộ phận trống
                if (string.IsNullOrWhiteSpace(boPhan.TenBoPhan))
                    return BadRequest("Tên bộ phận không được để trống");

                // Kiểm tra tên bộ phận đã tồn tại (trừ chính nó)
                var exists = await _context.BoPhan
                    .AnyAsync(b => b.TenBoPhan.ToLower() == boPhan.TenBoPhan.ToLower() 
                              && b.MaBoPhan != id);
                if (exists)
                    return BadRequest("Tên bộ phận đã tồn tại");

                _context.Entry(boPhan).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi cập nhật bộ phận: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var boPhan = await _context.BoPhan.FindAsync(id);
            if (boPhan == null) return NotFound();

            // Kiểm tra xem bộ phận có nhân viên không
            var coNhanVien = await _context.NhanVien
                .AnyAsync(nv => nv.MaBoPhan == id);

            if (coNhanVien)
                return BadRequest("Không thể xóa bộ phận đang có nhân viên");

            _context.BoPhan.Remove(boPhan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BoPhanExists(int id)
        {
            return _context.BoPhan.Any(e => e.MaBoPhan == id);
        }
    }
}