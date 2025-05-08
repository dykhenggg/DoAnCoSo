using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatBanController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        public DatBanController(RestaurantDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DatBan>>> Get() => 
            await _context.DatBan
                .Include(d => d.KhachHang)
                .Include(d => d.Ban)
                .ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<DatBan>> Get(int id)
        {
            var datBan = await _context.DatBan
                .Include(d => d.KhachHang)
                .Include(d => d.Ban)
                .FirstOrDefaultAsync(d => d.MaDatBan == id);
            return datBan == null ? NotFound() : datBan;
        }

        [HttpPost]
        public async Task<ActionResult<DatBan>> Post(DatBan datBan)
        {
            _context.DatBan.Add(datBan);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = datBan.MaDatBan }, datBan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, DatBan datBan)
        {
            if (id != datBan.MaDatBan) return BadRequest();
            _context.Entry(datBan).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var datBan = await _context.DatBan.FindAsync(id);
            if (datBan == null) return NotFound();
            _context.DatBan.Remove(datBan);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("by-date")]
        public async Task<ActionResult<IEnumerable<DatBan>>> GetByDate(DateTime date)
        {
            return await _context.DatBan
                .Include(d => d.KhachHang)
                .Include(d => d.Ban)
                .Where(d => d.NgayDat.Date == date.Date)
                .ToListAsync();
        }

        [HttpGet("by-customer")]
        public async Task<ActionResult<IEnumerable<DatBan>>> GetByCustomer(int customerId)
        {
            return await _context.DatBan
                .Include(d => d.KhachHang)
                .Include(d => d.Ban)
                .Where(d => d.MaKH == customerId)
                .ToListAsync();
        }
    }
}