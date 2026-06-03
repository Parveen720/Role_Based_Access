using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAccess.Models.DTOs;
using RoleBasedAccess.Models.Entities;
using RoleBasedAccess.Repositories;
using RoleBasedAccess.Services;

namespace RoleBasedAccess.Controllers;

public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;

    public AuthController(
        IUserRepository userRepository,
        JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _userRepository.EmailExistsAsync(dto.Email))
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

        await _userRepository.AddUserAsync(user);

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null)
        {
            ViewBag.Error = "Invalid Email";
            return View();
        }

        bool isValid = BCrypt.Net.BCrypt.Verify(
            dto.Password,
            user.PasswordHash);

        if (!isValid)
        {
            ViewBag.Error = "Invalid Password";
            return View();
        }

        string token = _jwtService.GenerateToken(user);

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
    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");

        return RedirectToAction("Login");
    }
}