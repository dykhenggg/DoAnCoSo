using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.Services;
using Backend.DTOs;
using Backend.Models.Enums;
using Microsoft.Extensions.Logging;
using System.Text.Json;

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
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var nhanVien = await _context.NhanVien
                .Include(n => n.BoPhan)
                .Select(n => new
                {
                    n.MaNV,
                    n.HoTen,
                    n.Email,
                    n.DiaChi,
                    n.ChucVu,
                    BoPhan = new
                    {
                        n.BoPhan.MaBoPhan,
                        n.BoPhan.TenBoPhan
                    }
                })
                .ToListAsync();

            return Ok(nhanVien);
        }

        [HttpPost]
        public async Task<ActionResult<object>> Create(NhanVienDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.hoTen))
                return BadRequest("Họ tên không được để trống");

            if (string.IsNullOrWhiteSpace(dto.email))
                return BadRequest("Email không được để trống");

            var emailExists = await _context.NhanVien
                .AnyAsync(n => n.Email.ToLower() == dto.email.ToLower());
            if (emailExists)
                return BadRequest("Email đã được sử dụng");

            var boPhan = await _context.BoPhan.FindAsync(dto.maBoPhan);
            if (boPhan == null)
                return BadRequest("Bộ phận không tồn tại");

            var nhanVien = new NhanVien
            {
                HoTen = dto.hoTen.Trim(),
                Email = dto.email.Trim().ToLower(),
                DiaChi = dto.diaChi?.Trim() ?? string.Empty,
                ChucVu = dto.chucVu,
                MatKhau = _authService.HashPassword(dto.matKhau ?? "123456"),
                MaBoPhan = dto.maBoPhan
            };

            _context.NhanVien.Add(nhanVien);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                nhanVien.MaNV,
                nhanVien.HoTen,
                nhanVien.Email,
                nhanVien.DiaChi,
                nhanVien.ChucVu,
                BoPhan = new
                {
                    boPhan.MaBoPhan,
                    boPhan.TenBoPhan
                }
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NhanVien>> UpdateNhanVien(int id, NhanVienDTO nhanVienDTO)
        {
            try
            {
                var existingNhanVien = await _context.NhanVien
                    .Include(nv => nv.BoPhan)
                    .FirstOrDefaultAsync(nv => nv.MaNV == id);

                if (existingNhanVien == null)
                {
                    return NotFound($"Không tìm thấy nhân viên với mã {id}");
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(nhanVienDTO.hoTen))
                {
                    return BadRequest("Họ tên không được để trống");
                }
                if (string.IsNullOrWhiteSpace(nhanVienDTO.email))
                {
                    return BadRequest("Email không được để trống");
                }

                // Check if email is unique (excluding current employee)
                var emailExists = await _context.NhanVien
                    .AnyAsync(nv => nv.Email == nhanVienDTO.email && nv.MaNV != id);
                if (emailExists)
                {
                    return BadRequest("Email đã tồn tại trong hệ thống");
                }

                // Check if department exists
                var boPhan = await _context.BoPhan.FindAsync(nhanVienDTO.maBoPhan);
                if (boPhan == null)
                {
                    return BadRequest($"Không tìm thấy bộ phận với mã {nhanVienDTO.maBoPhan}");
                }

                // Update employee information
                existingNhanVien.HoTen = nhanVienDTO.hoTen.Trim();
                existingNhanVien.Email = nhanVienDTO.email.Trim().ToLower();
                existingNhanVien.DiaChi = nhanVienDTO.diaChi?.Trim();
                existingNhanVien.ChucVu = nhanVienDTO.chucVu;
                existingNhanVien.MaBoPhan = nhanVienDTO.maBoPhan;

                await _context.SaveChangesAsync();

                // Return updated employee with department info
                return Ok(await _context.NhanVien
                    .Include(nv => nv.BoPhan)
                    .FirstAsync(nv => nv.MaNV == id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi cập nhật nhân viên: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var nhanVien = await _context.NhanVien.FindAsync(id);
            if (nhanVien == null)
                return NotFound("Không tìm thấy nhân viên");

            _context.NhanVien.Remove(nhanVien);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NhanVien>> GetById(int id)
        {
            var nhanVien = await _context.NhanVien
                .Include(n => n.BoPhan)
                .FirstOrDefaultAsync(n => n.MaNV == id);

            if (nhanVien == null)
                return NotFound("Không tìm thấy nhân viên");

            return Ok(new { 
                nhanVien.MaNV,
                nhanVien.HoTen,
                nhanVien.Email,
                nhanVien.DiaChi,
                nhanVien.ChucVu,
                nhanVien.MaBoPhan,
                TenBoPhan = nhanVien.BoPhan?.TenBoPhan
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO dto)
        {
            var nhanVien = await _context.NhanVien
                .FirstOrDefaultAsync(n => n.Email == dto.TenDangNhap);

            if (nhanVien == null || !_authService.VerifyPassword(dto.MatKhau, nhanVien.MatKhau))
                return Unauthorized("Email hoặc mật khẩu không đúng");

            if (Enum.TryParse<UserRoles>(nhanVien.ChucVu, true, out UserRoles chucVu) && chucVu == UserRoles.NhanVien)
                return Unauthorized("Tài khoản không có quyền truy cập");

            var token = _authService.GenerateJwtToken(nhanVien);
            return Ok(new { token, nhanVien });
        }

        [HttpPut("{id}/trangthai")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] bool trangThai)
        {
            var nhanVien = await _context.NhanVien.FindAsync(id);
            if (nhanVien == null) return NotFound();
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("by-department/{departmentId}")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetByDepartment(int departmentId)
        {
            try
            {
                var nhanVien = await _context.NhanVien
                    .Include(n => n.BoPhan)
                    .Select(n => new
                    {
                        n.MaNV,
                        n.HoTen,
                        n.Email,
                        n.DiaChi
                    })
                    .ToListAsync();

                return Ok(nhanVien);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy danh sách nhân viên: {ex.Message}");
            }
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetEmployeeCount()
        {
            return await _context.NhanVien.CountAsync();
        }
    }
}
