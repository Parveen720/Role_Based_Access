using Microsoft.EntityFrameworkCore;
using RoleBasedAccess.Data;
using RoleBasedAccess.Models.Entities;
using RoleBasedAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(x => x.Email == email);
    }

    public async Task<List<User>> GetStudentsAsync()
    {
        return await _context.Users
            .Where(x => x.Role == "Student")
            .ToListAsync();
    }

    public async Task<List<User>> GetFacultiesAsync()
    {
        return await _context.Users
            .Where(x => x.Role == "Faculty")
            .ToListAsync();
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}