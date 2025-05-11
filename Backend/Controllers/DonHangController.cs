using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public DonHangController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonHang>>> GetDonHang()
        {
            return await _context.DonHang
                .Include(d => d.ChiTietDonHang)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<DonHang>> CreateDonHang(DonHang donHang)
        {
            _context.DonHang.Add(donHang);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDonHang), new { id = donHang.MaDonHang }, donHang);
        }
    }
}
