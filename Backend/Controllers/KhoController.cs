using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Data;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kho>>> GetKho()
        {
            return await _context.Kho.Include(k => k.GiaoDichKhos).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Kho>> GetKho(int id)
        {
            var kho = await _context.Kho.FindAsync(id);
            if (kho == null) return NotFound();
            return kho;
        }

        [HttpPost]
        public async Task<ActionResult<Kho>> CreateKho(Kho kho)
        {
            _context.Kho.Add(kho);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetKho), new { id = kho.MaNguyenLieu }, kho);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKho(int id, Kho kho)
        {
            if (id != kho.MaNguyenLieu) return BadRequest();
            _context.Entry(kho).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
