using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaLamViecController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public CaLamViecController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaLamViec>>> GetCaLamViec()
        {
            return await _context.CaLamViec
                .Include(c => c.NhanVien)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<CaLamViec>> CreateCaLamViec(CaLamViec caLamViec)
        {
            _context.CaLamViec.Add(caLamViec);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCaLamViec), new { id = caLamViec.MaCa }, caLamViec);
        }
    }
}
