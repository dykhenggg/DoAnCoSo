using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Data;

namespace Backend.Controllers
{
    public class KhachHangController : BaseController
    {
        public KhachHangController(RestaurantDbContext context) : base(context) { }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhachHang>>> GetAll()
        {
            return await _context.KhachHang
                .Include(k => k.DonHang)
                .Include(k => k.DatBan)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<KhachHang>> Create(KhachHang khachHang)
        {
            _context.KhachHang.Add(khachHang);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = khachHang.MaKH }, khachHang);
        }
    }
}
