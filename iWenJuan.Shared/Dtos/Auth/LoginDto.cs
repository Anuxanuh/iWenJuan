using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWenJuan.Shared.Dtos;

public class LoginDto
{
	[Required]
	[EmailAddress(ErrorMessage = "邮箱格式不正确")]
	public string UserEmail { get; set; }
	[Required]
	public string UserPassword { get; set; }
}
