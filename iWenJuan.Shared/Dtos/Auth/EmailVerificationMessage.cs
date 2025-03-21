using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWenJuan.Shared.Dtos;

public class EmailVerificationMessage
{
	/// <summary>
	/// 电子邮件地址。
	/// </summary>
	public string Email { get; set; }

	/// <summary>
	/// 验证码。
	/// </summary>
	public string Code { get; set; }
}
