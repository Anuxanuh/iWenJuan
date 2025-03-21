using System.ComponentModel.DataAnnotations;

namespace iWenJuan.Shared.Dtos;

public class UserDto
{
	public Guid Id { get; set; }

	public string UserName { get; set; }

	[EmailAddress]
	public string Email { get; set; }

	public DateTime CreatedAt { get; set; }
}
