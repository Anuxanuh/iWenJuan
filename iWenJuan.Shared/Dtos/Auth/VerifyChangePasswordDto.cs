using System.ComponentModel.DataAnnotations;

namespace iWenJuan.Shared.Dtos;

public class VerifyChangePasswordDto
{
	[EmailAddress(ErrorMessage = "邮箱格式不正确")]
	public string UserEmail { get; set; }
	public string VerificationCode { get; set; }
	public string NewPassword { get; set; }
}
