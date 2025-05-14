using Assessment.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Assessment.DataAccess.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
}