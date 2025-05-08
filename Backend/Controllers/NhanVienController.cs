using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NhanVienController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        public NhanVienController(RestaurantDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhanVien>>> Get() => 
            await _context.NhanVien
                .Include(n => n.CaLamViec)
                .ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<NhanVien>> Get(int id)
        {
            var nhanVien = await _context.NhanVien
                .Include(n => n.CaLamViec)
                .FirstOrDefaultAsync(n => n.MaNV == id);
            return nhanVien == null ? NotFound() : nhanVien;
        }

        [HttpPost]
        public async Task<ActionResult<NhanVien>> Post(NhanVien nhanVien)
        {
            _context.NhanVien.Add(nhanVien);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = nhanVien.MaNV }, nhanVien);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, NhanVien nhanVien)
        {
            if (id != nhanVien.MaNV) return BadRequest();
            _context.Entry(nhanVien).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var nhanVien = await _context.NhanVien.FindAsync(id);
            if (nhanVien == null) return NotFound();
            _context.NhanVien.Remove(nhanVien);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("by-role")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetByRole(string role)
        {
            return await _context.NhanVien
                .Where(n => n.ChucVu == role)
                .ToListAsync();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> Search(string keyword)
        {
            return await _context.NhanVien
                .Where(n => n.HoTen.Contains(keyword) || n.Email.Contains(keyword))
                .ToListAsync();
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetAvailable(DateTime date)
        {
            return await _context.NhanVien
                .Include(n => n.CaLamViec)
                .Where(n => n.TrangThai == "Đang làm")
                .ToListAsync();
        }
    }
}