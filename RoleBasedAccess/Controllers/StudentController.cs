using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAccess.Repositories;
using System.Security.Claims;

namespace RoleBasedAccess.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly IUserRepository _userRepository;

        public StudentController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var student =
                await _userRepository.GetByEmailAsync(email);

            return View(student);
        }
    }
}