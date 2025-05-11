using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public BanController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ban>>> GetBan()
        {
            return await _context.Ban.Include(b => b.DatBan).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Ban>> CreateBan(Ban ban)
        {
            _context.Ban.Add(ban);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBan), new { id = ban.MaBan }, ban);
        }
    }
}
