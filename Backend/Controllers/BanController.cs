using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BanController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        public BanController(RestaurantDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ban>>> Get() => await _context.Ban.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Ban>> Get(int id)
        {
            var item = await _context.Ban.FindAsync(id);
            return item == null ? NotFound() : item;
        }

        [HttpPost]
        public async Task<ActionResult<Ban>> Post(Ban item)
        {
            _context.Ban.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = item.MaBan }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Ban item)
        {
            if (id != item.MaBan) return BadRequest();
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Ban.FindAsync(id);
            if (item == null) return NotFound();
            _context.Ban.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Ban>>> Search(string keyword)
        {
            return await _context.Ban
                .Where(x => x.TenBan.Contains(keyword))
                .ToListAsync();
        }

        [HttpGet("page")]
        public async Task<ActionResult<IEnumerable<Ban>>> Paginate(int page = 1, int pageSize = 10)
        {
            return await _context.Ban
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        [HttpPost("delete-multiple")]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<int> ids)
        {
            var items = await _context.Ban.Where(x => ids.Contains(x.MaBan)).ToListAsync();
            _context.Ban.RemoveRange(items);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
