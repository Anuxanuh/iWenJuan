namespace iWenJuan.Service.Auth.Interface;

/// <summary>
/// Redis服务接口，用于处理验证码。
/// </summary>
public interface IRedisService
{
	/// <summary>
	/// 设置指定邮箱的验证码及其过期时间。
	/// </summary>
	/// <param name="email">要设置验证码的邮箱地址。</param>
	/// <param name="code">要设置的验证码。</param>
	/// <param name="expiry">验证码的过期时间。</param>
	/// <returns>表示异步操作的任务。</returns>
	Task SetVerificationCodeAsync(string email, string code, TimeSpan expiry);

	/// <summary>
	/// 获取指定邮箱的验证码。
	/// </summary>
	/// <param name="email">要获取验证码的邮箱地址。</param>
	/// <returns>表示异步操作的任务。任务结果包含验证码。</returns>
	Task<string> GetVerificationCodeAsync(string email);

	/// <summary>
	/// 验证指定邮箱的验证码。
	/// </summary>
	/// <param name="email">要验证验证码的邮箱地址。</param>
	/// <param name="code">要验证的验证码。</param>
	/// <returns>表示异步操作的任务。任务结果包含一个布尔值，指示验证是否成功。</returns>
	Task<bool> VerifyVerificationCodeAsync(string email, string code);
}
