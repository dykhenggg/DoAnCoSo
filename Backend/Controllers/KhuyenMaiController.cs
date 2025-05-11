using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public KhuyenMaiController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhuyenMai>>> GetKhuyenMai()
        {
            return await _context.KhuyenMai.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<KhuyenMai>> CreateKhuyenMai(KhuyenMai khuyenMai)
        {
            _context.KhuyenMai.Add(khuyenMai);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetKhuyenMai), new { id = khuyenMai.MaKM }, khuyenMai);
        }
    }
}
