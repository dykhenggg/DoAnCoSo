using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThucDonController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        public ThucDonController(RestaurantDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThucDon>>> Get() => await _context.ThucDon.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<ThucDon>> Get(int id)
        {
            var item = await _context.ThucDon.FindAsync(id);
            return item == null ? NotFound() : item;
        }

        [HttpPost]
        public async Task<ActionResult<ThucDon>> Post(ThucDon item)
        {
            _context.ThucDon.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = item.MaMon }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ThucDon item)
        {
            if (id != item.MaMon) return BadRequest();
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.ThucDon.FindAsync(id);
            if (item == null) return NotFound();
            _context.ThucDon.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ThucDon>>> Search(string keyword)
        {
            return await _context.ThucDon
                .Where(x => x.TenMon.Contains(keyword))
                .ToListAsync();
        }

        [HttpGet("page")]
        public async Task<ActionResult<IEnumerable<ThucDon>>> Paginate(int page = 1, int pageSize = 10)
        {
            return await _context.ThucDon
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        [HttpPost("delete-multiple")]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<int> ids)
        {
            var items = await _context.ThucDon.Where(x => ids.Contains(x.MaMon)).ToListAsync();
            _context.ThucDon.RemoveRange(items);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}