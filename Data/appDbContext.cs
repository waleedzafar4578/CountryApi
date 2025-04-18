using Microsoft.EntityFrameworkCore;
using CountryApi.Models;

namespace CountryApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Country> Countries { get; set; }
    public DbSet<User> Users { get; set; }
}