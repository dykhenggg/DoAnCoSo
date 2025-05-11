using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatBanController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public DatBanController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DatBan>>> GetDatBan()
        {
            return await _context.DatBan
                .Include(d => d.KhachHang)
                .Include(d => d.Ban)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<DatBan>> CreateDatBan(DatBan datBan)
        {
            _context.DatBan.Add(datBan);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDatBan), new { id = datBan.MaDatBan }, datBan);
        }
    }
}
