namespace iWenJuan.Service.Auth.Interface;

public interface IJwtTokenHelper<T>
{
	/// <summary>
	/// 为指定用户生成JWT令牌。
	/// </summary>
	/// <param name="user">生成令牌的用户。</param>
	/// <param name="key">用于签署令牌的密钥。</param>
	/// <param name="expireMinutes">令牌的过期时间（分钟）。</param>
	/// <returns>生成的JWT令牌字符串。</returns>
	string GenerateToken(T user);

	/// <summary>
	/// 验证JWT令牌的有效性。
	/// </summary>
	/// <param name="token">要验证的JWT令牌。</param>
	/// <returns>如果令牌有效，返回用户id, 否则返回null</returns>
	(string,string) ValidateToken(string token);
}
