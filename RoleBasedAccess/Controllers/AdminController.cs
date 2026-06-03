using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAccess.Repositories;

namespace RoleBasedAccess.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AdminController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> Students()
        {
            var students = await _userRepository.GetStudentsAsync();

            return View(students);
        }

        public async Task<IActionResult> Faculties()
        {
            var faculties = await _userRepository.GetFacultiesAsync();

            return View(faculties);
        }
    }
}