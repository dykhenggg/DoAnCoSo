using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KhoController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        public KhoController(RestaurantDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kho>>> Get() => 
            await _context.Kho.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Kho>> Get(int id)
        {
            var kho = await _context.Kho.FindAsync(id);
            return kho == null ? NotFound() : kho;
        }

        [HttpPost]
        public async Task<ActionResult<Kho>> Post(Kho kho)
        {
            _context.Kho.Add(kho);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = kho.MaNguyenLieu }, kho);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Kho kho)
        {
            if (id != kho.MaNguyenLieu) return BadRequest();
            _context.Entry(kho).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var kho = await _context.Kho.FindAsync(id);
            if (kho == null) return NotFound();
            _context.Kho.Remove(kho);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<Kho>>> GetLowStock()
        {
            return await _context.Kho
                .Where(k => k.SoLuongHienTai <= k.SoLuongToiThieu)
                .ToListAsync();
        }

        [HttpPut("update-stock")]
        public async Task<IActionResult> UpdateStock(int id, decimal quantity)
        {
            var kho = await _context.Kho.FindAsync(id);
            if (kho == null) return NotFound();
            
            kho.SoLuongHienTai = quantity;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("by-category")]
        public async Task<ActionResult<IEnumerable<Kho>>> GetByCategory(string category)
        {
            return await _context.Kho
                .Where(k => k.DanhMuc == category)
                .ToListAsync();
        }
    }
}