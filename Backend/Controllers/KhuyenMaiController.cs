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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhuyenMai>>> GetAll()
        {
            return await _context.KhuyenMai
                .OrderByDescending(k => k.NgayBatDau)
                .ToListAsync();
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
        public async Task<ActionResult<KhuyenMai>> Create(KhuyenMai khuyenMai)
        {
            _context.KhuyenMai.Add(khuyenMai);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetActive), new { id = khuyenMai.MaKM }, khuyenMai);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, KhuyenMai khuyenMai)
        {
            if (id != khuyenMai.MaKM) return BadRequest();

            _context.Entry(khuyenMai).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var khuyenMai = await _context.KhuyenMai.FindAsync(id);
            if (khuyenMai == null) return NotFound();

            _context.KhuyenMai.Remove(khuyenMai);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
