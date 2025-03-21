using iWenJuan.Service.Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace iWenJuan.Service.Auth.Data;

public class AuthDbContext : DbContext
{
	public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
	{
	}
	public DbSet<User> Users { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
	}
}
