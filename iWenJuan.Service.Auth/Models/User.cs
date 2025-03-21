using System.ComponentModel.DataAnnotations;

namespace iWenJuan.Service.Auth.Models;

public class User
{
	public Guid Id { get; set; } = Guid.NewGuid();

	[Required]
	public string UserName { get; set; }

	[Required]
	public string PasswordHash { get; set; }

	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
