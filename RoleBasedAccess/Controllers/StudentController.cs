using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAccess.Data;
using System.Security.Claims;

namespace RoleBasedAccess.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Profile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var student = _context.Users
                .FirstOrDefault(x => x.Email == email);

            return View(student);
        }
    }
}