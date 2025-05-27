using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Backend.Models;
using Backend.Models.Enums;

namespace Backend.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public string GenerateJwtToken(NhanVien nhanVien)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, nhanVien.MaNV.ToString()),
                new Claim(ClaimTypes.Name, nhanVien.Email),
                new Claim(ClaimTypes.Role, nhanVien.ChucVu)
            };

            return GenerateToken(claims);
        }

        public string GenerateJwtToken(TaiKhoan taiKhoan)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, taiKhoan.MaTaiKhoan.ToString()),
                new Claim(ClaimTypes.Name, taiKhoan.TenDangNhap),
                new Claim(ClaimTypes.Role, taiKhoan.VaiTro.TenVaiTro)
            };

            return GenerateToken(claims);
        }

        private string GenerateToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? ""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool IsAdmin(string role)
        {
            return role == UserRoles.QuanLy;
        }

        public bool IsEmployee(string role)
        {
            return role == UserRoles.NhanVien;
        }
    }
}
