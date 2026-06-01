using Microsoft.EntityFrameworkCore;
using RoleBasedAccess.Models.Entities;

namespace RoleBasedAccess.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}