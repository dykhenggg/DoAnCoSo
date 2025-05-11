using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiaoDichKhoController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public GiaoDichKhoController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GiaoDichKho>>> GetGiaoDichKho()
        {
            return await _context.GiaoDichKho.Include(g => g.Kho).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<GiaoDichKho>> CreateGiaoDichKho(GiaoDichKho giaoDich)
        {
            _context.GiaoDichKho.Add(giaoDich);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGiaoDichKho), new { id = giaoDich.MaGiaoDich }, giaoDich);
        }
    }
}
