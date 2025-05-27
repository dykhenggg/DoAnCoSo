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
            try
            {
                var nhanVien = await _context.NhanVien
                    .Include(n => n.BoPhan)
                    .Select(n => new
                    {
                        n.MaNV,
                        n.HoTen,
                        n.Email,
                        n.SDT,
                        n.DiaChi,
                        n.ChucVu,
                        BoPhan = n.BoPhan != null ? new
                        {
                            n.BoPhan.MaBoPhan,
                            n.BoPhan.TenBoPhan
                        } : null
                    })
                    .ToListAsync();

                return Ok(nhanVien);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lấy danh sách nhân viên: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO dto)
        {
            var nhanVien = await _context.NhanVien
                .FirstOrDefaultAsync(n => n.Email == dto.TenDangNhap);

            if (nhanVien == null || !_authService.VerifyPassword(dto.MatKhau, nhanVien.MatKhau))
                return Unauthorized("Email hoặc mật khẩu không đúng");

            if (nhanVien.ChucVu == UserRoles.NhanVien)
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

        [HttpPost]
        public async Task<ActionResult<NhanVien>> Create(NhanVienDTO dto)
        {
            try
            {
                // Kiểm tra email đã tồn tại
                var emailExists = await _context.NhanVien
                    .AnyAsync(n => n.Email.ToLower() == dto.Email.ToLower());
                if (emailExists)
                    return BadRequest("Email đã được sử dụng");

                // Kiểm tra số điện thoại đã tồn tại
                var sdtExists = await _context.NhanVien
                    .AnyAsync(n => n.SDT == dto.SDT);
                if (sdtExists)
                    return BadRequest("Số điện thoại đã được sử dụng");

                // Kiểm tra bộ phận tồn tại
                var boPhanExists = await _context.BoPhan
                    .AnyAsync(b => b.MaBoPhan == dto.MaBoPhan);
                if (!boPhanExists)
                    return BadRequest("Bộ phận không tồn tại");

                var nhanVien = new NhanVien
                {
                    HoTen = dto.HoTen,
                    Email = dto.Email,
                    SDT = dto.SDT,
                    DiaChi = dto.DiaChi,
                    ChucVu = dto.ChucVu,
                    MatKhau = _authService.HashPassword(dto.MatKhau),
                    MaBoPhan = dto.MaBoPhan
                };

                _context.NhanVien.Add(nhanVien);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = nhanVien.MaNV }, nhanVien);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi thêm nhân viên: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NhanVien nhanVien)
        {
            if (id != nhanVien.MaNV) return BadRequest();

            try
            {
                // Kiểm tra email đã tồn tại (trừ chính nó)
                var emailExists = await _context.NhanVien
                    .AnyAsync(n => n.Email.ToLower() == nhanVien.Email.ToLower() 
                              && n.MaNV != id);
                if (emailExists)
                    return BadRequest("Email đã được sử dụng");

                // Kiểm tra số điện thoại đã tồn tại (trừ chính nó)
                var sdtExists = await _context.NhanVien
                    .AnyAsync(n => n.SDT == nhanVien.SDT && n.MaNV != id);
                if (sdtExists)
                    return BadRequest("Số điện thoại đã được sử dụng");

                // Kiểm tra bộ phận tồn tại
                var boPhanExists = await _context.BoPhan
                    .AnyAsync(b => b.MaBoPhan == nhanVien.MaBoPhan);
                if (!boPhanExists)
                    return BadRequest("Bộ phận không tồn tại");

                _context.Entry(nhanVien).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi cập nhật nhân viên: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var nhanVien = await _context.NhanVien.FindAsync(id);
                if (nhanVien == null) return NotFound();

                // Kiểm tra xem nhân viên có liên quan đến dữ liệu khác không
                var coCaLamViec = await _context.CaLamViec
                    .AnyAsync(c => c.MaNhanVien == id);
                var coChamCong = await _context.ChamCong
                    .AnyAsync(c => c.MaNhanVien == id);

                if (coCaLamViec || coChamCong)
                    return BadRequest("Không thể xóa nhân viên đã có dữ liệu liên quan");

                _context.NhanVien.Remove(nhanVien);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi xóa nhân viên: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NhanVien>> GetById(int id)
        {
            try
            {
                var nhanVien = await _context.NhanVien
                    .Include(n => n.BoPhan)
                    .FirstOrDefaultAsync(n => n.MaNV == id);

                if (nhanVien == null) return NotFound();

                return nhanVien;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi lấy thông tin nhân viên: " + ex.Message);
            }
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
                        n.SDT,
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
