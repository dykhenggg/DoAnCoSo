using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KhachHangController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        public KhachHangController(RestaurantDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhachHang>>> Get() => await _context.KhachHang.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<KhachHang>> Get(int id)
        {
            var item = await _context.KhachHang.FindAsync(id);
            return item == null ? NotFound() : item;
        }

        [HttpPost]
        public async Task<ActionResult<KhachHang>> Post(KhachHang item)
        {
            _context.KhachHang.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = item.MaKhachHang }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, KhachHang item)
        {
            if (id != item.MaKhachHang) return BadRequest();
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.KhachHang.FindAsync(id);
            if (item == null) return NotFound();
            _context.KhachHang.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
