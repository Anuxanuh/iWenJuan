using iWenJuan.Service.DataStorage.Models;
using Microsoft.EntityFrameworkCore;

namespace iWenJuan.Service.DataStorage.Data;

public class StorageDbContext : DbContext
{
	public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options)
	{
	}
	public DbSet<StoredFile> StoredFiles { get; set; }
}
