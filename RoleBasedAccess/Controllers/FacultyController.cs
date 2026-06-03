using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAccess.Repositories;

namespace RoleBasedAccess.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class FacultyController : Controller
    {
        private readonly IUserRepository _userRepository;

        public FacultyController(IUserRepository userRepository)
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
    }
}