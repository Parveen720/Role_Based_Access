using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAccess.Data;
using RoleBasedAccess.Models.DTOs;
using RoleBasedAccess.Models.Entities;
using RoleBasedAccess.Services;

namespace RoleBasedAccess.Controllers;

public class AuthController : Controller
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(
        AppDbContext context,
        JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterDto dto)
    {
        if (_context.Users.Any(x => x.Email == dto.Email))
        {
            ViewBag.Error = "Email already exists";
            return View();
        }

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginDto dto)
    {
        var user = _context.Users
            .FirstOrDefault(x => x.Email == dto.Email);

        if (user == null)
        {
            ViewBag.Error = "Invalid Email";
            return View();
        }

        bool isValid =
            BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash);

        if (!isValid)
        {
            ViewBag.Error = "Invalid Password";
            return View();
        }

        string token =
            _jwtService.GenerateToken(user);

        Response.Cookies.Append(
            "jwt",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            });

        if (user.Role == "Admin")
        {
            return RedirectToAction(
                "Dashboard",
                "Admin");
        }

        if (user.Role == "Faculty")
        {
            return RedirectToAction(
                "Dashboard",
                "Faculty");
        }

        return RedirectToAction(
            "Dashboard",
            "Student");
    }
}