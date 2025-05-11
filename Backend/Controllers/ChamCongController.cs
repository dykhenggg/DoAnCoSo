using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChamCongController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public ChamCongController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChamCong>>> GetChamCong()
        {
            return await _context.ChamCong
                .Include(c => c.NhanVien)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChamCong>> GetChamCong(int id)
        {
            var chamCong = await _context.ChamCong
                .Include(c => c.NhanVien)
                .FirstOrDefaultAsync(c => c.MaChamCong == id);

            if (chamCong == null) return NotFound();
            return chamCong;
        }

        [HttpPost]
        public async Task<ActionResult<ChamCong>> CreateChamCong(ChamCong chamCong)
        {
            chamCong.ThoiGian = DateTime.Now;
            _context.ChamCong.Add(chamCong);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetChamCong), new { id = chamCong.MaChamCong }, chamCong);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChamCong(int id, ChamCong chamCong)
        {
            if (id != chamCong.MaChamCong) return BadRequest();
            _context.Entry(chamCong).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
