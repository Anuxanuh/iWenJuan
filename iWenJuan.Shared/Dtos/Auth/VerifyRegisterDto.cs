using System.ComponentModel.DataAnnotations;

namespace iWenJuan.Shared.Dtos;

public class VerifyRegisterDto
{
	[Required]
	public string UserName { get; set; }
	[Required]
	[EmailAddress(ErrorMessage = "�����ʽ����ȷ")]
	public string UserEmail { get; set; }
	[Required]
	[DataType(DataType.Password)]
	public string UserPassword { get; set; }
	public string VerificationCode { get; set; }
}
