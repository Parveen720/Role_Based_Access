using RoleBasedAccess.Models.Entities;

namespace RoleBasedAccess.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task<bool> EmailExistsAsync(string email);

        Task<List<User>> GetStudentsAsync();

        Task<List<User>> GetFacultiesAsync();

        Task AddUserAsync(User user);
    }
}
