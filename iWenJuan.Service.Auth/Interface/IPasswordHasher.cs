namespace iWenJuan.Service.Auth.Interface;

public interface IPasswordHasher
{
	/// <summary>
	/// 哈希密码并返回盐值和哈希值的组合字符串
	/// </summary>
	/// <param name="password">待hash的密码</param>
	/// <returns>返回盐值和哈希值的组合字符串</returns>
	public string HashPassword(string password);

	/// <summary>
	/// 验证输入的密码是否与存储的哈希值匹配
	/// </summary>
	/// <param name="storedHash">存储的哈希值</param>
	/// <param name="password">输入的密码</param>
	/// <returns>如果匹配返回 true，否则返回 false</returns>
	public bool VerifyPassword(string storedHash, string password);
}
