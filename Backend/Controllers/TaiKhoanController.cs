using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.Services;
using Backend.DTOs;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiKhoanController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        private readonly AuthService _authService;

        public TaiKhoanController(RestaurantDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaiKhoanDTO>>> GetAll()
        {
            var taiKhoans = await _context.TaiKhoan
                .Include(t => t.VaiTro)
                .Select(t => new TaiKhoanDTO
                {
                    MaTaiKhoan = t.MaTaiKhoan,
                    HoTen = t.HoTen,
                    TenDangNhap = t.TenDangNhap,
                    Email = t.Email,
                    SDT = t.SDT,
                    TrangThai = t.TrangThai,
                    NgayThamGia = t.NgayThamGia,
                    TenVaiTro = t.VaiTro.TenVaiTro
                })
                .ToListAsync();

            return Ok(taiKhoans);
        }

        [HttpPost("register")]
        public async Task<ActionResult<TaiKhoanDTO>> Register(RegisterTaiKhoanDTO dto)
        {
            // Kiểm tra tên đăng nhập đã tồn tại
            if (await _context.TaiKhoan.AnyAsync(t => t.TenDangNhap == dto.TenDangNhap))
                return BadRequest("Tên đăng nhập đã tồn tại");

            // Kiểm tra email đã tồn tại
            if (await _context.TaiKhoan.AnyAsync(t => t.Email == dto.Email))
                return BadRequest("Email đã được sử dụng");

            // Kiểm tra số điện thoại đã tồn tại
            if (await _context.TaiKhoan.AnyAsync(t => t.SDT == dto.SDT))
                return BadRequest("Số điện thoại đã được sử dụng");

            var taiKhoan = new TaiKhoan
            {
                HoTen = dto.HoTen,
                TenDangNhap = dto.TenDangNhap,
                Email = dto.Email,
                SDT = dto.SDT,
                MatKhauHash = _authService.HashPassword(dto.MatKhau),
                MaVaiTro = dto.MaVaiTro
            };

            _context.TaiKhoan.Add(taiKhoan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new TaiKhoanDTO
            {
                MaTaiKhoan = taiKhoan.MaTaiKhoan,
                HoTen = taiKhoan.HoTen,
                TenDangNhap = taiKhoan.TenDangNhap,
                Email = taiKhoan.Email,
                SDT = taiKhoan.SDT,
                TrangThai = taiKhoan.TrangThai,
                NgayThamGia = taiKhoan.NgayThamGia
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO dto)
        {
            try
            {
                var taiKhoan = await _context.TaiKhoan
                    .Include(t => t.VaiTro)
                    .FirstOrDefaultAsync(t => t.TenDangNhap == dto.TenDangNhap);

                if (taiKhoan == null)
                    return Unauthorized("Tên đăng nhập hoặc mật khẩu không đúng");

                if (!_authService.VerifyPassword(dto.MatKhau, taiKhoan.MatKhauHash))
                {
                    taiKhoan.LoginAttempts++;
                    if (taiKhoan.LoginAttempts >= 5)
                    {
                        taiKhoan.TrangThai = "Locked";
                        taiKhoan.LockoutEnd = DateTime.UtcNow.AddMinutes(30);
                    }
                    await _context.SaveChangesAsync();
                    return Unauthorized("Tên đăng nhập hoặc mật khẩu không đúng");
                }

                if (taiKhoan.TrangThai == "Locked")
                {
                    if (taiKhoan.LockoutEnd > DateTime.UtcNow)
                        return Unauthorized($"Tài khoản đã bị khóa. Vui lòng thử lại sau {Math.Ceiling((taiKhoan.LockoutEnd.Value - DateTime.UtcNow).TotalMinutes)} phút");
                    
                    taiKhoan.TrangThai = "Active";
                    taiKhoan.LoginAttempts = 0;
                    taiKhoan.LockoutEnd = null;
                }

                if (taiKhoan.TrangThai != "Active")
                    return Unauthorized("Tài khoản đã bị vô hiệu hóa");

                taiKhoan.LastLogin = DateTime.UtcNow;
                taiKhoan.LoginAttempts = 0;
                await _context.SaveChangesAsync();

                var token = _authService.GenerateJwtToken(taiKhoan);

                return Ok(new
                {
                    token,
                    taiKhoan = new TaiKhoanDTO
                    {
                        MaTaiKhoan = taiKhoan.MaTaiKhoan,
                        HoTen = taiKhoan.HoTen,
                        TenDangNhap = taiKhoan.TenDangNhap,
                        Email = taiKhoan.Email,
                        SDT = taiKhoan.SDT,
                        TrangThai = taiKhoan.TrangThai,
                        NgayThamGia = taiKhoan.NgayThamGia,
                        TenVaiTro = taiKhoan.VaiTro.TenVaiTro
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi đăng nhập: {ex.Message}");
            }
        }
    }
}