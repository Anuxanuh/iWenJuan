namespace iWenJuan.Service.Auth.Interface;

/// <summary>
/// 验证码服务接口。
/// </summary>
public interface IVerificationCodeService
{
	/// <summary>
	/// 为指定的电子邮件生成验证码，并发送验证码。
	/// 该方法会生成一个随机验证码，并通过电子邮件发送给用户。
	/// </summary>
	/// <param name="email">接收验证码的电子邮件地址。</param>
	/// <returns>返回生成的验证码。</returns>
	Task<string> GetAndSendVerificationCode(string email);

	/// <summary>
	/// 验证指定电子邮件的验证码。
	/// </summary>
	/// <param name="email">要验证验证码的电子邮件地址。</param>
	/// <param name="code">要验证的验证码。</param>
	/// <returns>如果验证码有效，则返回 true；否则返回 false。</returns>
	Task<bool> VerifyVerificationCode(string email, string code);
}
