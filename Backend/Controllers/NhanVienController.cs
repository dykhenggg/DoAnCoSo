using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.Services;
using Backend.DTOs;
using Backend.Models.Enums;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        private readonly AuthService _authService;

        public NhanVienController(RestaurantDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetAll()
        {
            return await _context.NhanVien
                .Include(n => n.BoPhan)
                .Where(n => n.TrangThai == "Đang làm việc")
                .ToListAsync();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO dto)
        {
            var nhanVien = await _context.NhanVien
                .FirstOrDefaultAsync(n => n.Email == dto.Email);

            if (nhanVien == null || !_authService.VerifyPassword(dto.Password, nhanVien.MatKhau))
                return Unauthorized("Email hoặc mật khẩu không đúng");

            if (nhanVien.ChucVu == UserRoles.KhachHang)
                return Unauthorized("Tài khoản không có quyền truy cập");

            var token = _authService.GenerateJwtToken(nhanVien);
            return Ok(new { token, nhanVien });
        }

        [HttpPut("{id}/trangthai")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string trangThai)
        {
            var nhanVien = await _context.NhanVien.FindAsync(id);
            if (nhanVien == null) return NotFound();

            nhanVien.TrangThai = trangThai;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
