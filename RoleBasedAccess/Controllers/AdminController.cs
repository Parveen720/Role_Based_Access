using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAccess.Data;

namespace RoleBasedAccess.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Students()
        {
            var students = _context.Users
                .Where(x => x.Role == "Student")
                .ToList();

            return View(students);
        }

        public IActionResult Faculties()
        {
            var faculties = _context.Users
                .Where(x => x.Role == "Faculty")
                .ToList();

            return View(faculties);
        }
    }
}