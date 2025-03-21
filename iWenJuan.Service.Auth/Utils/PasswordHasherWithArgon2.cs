using iWenJuan.Service.Auth.Interface;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace iWenJuan.Service.Auth.Utils;

/// <summary>
/// 提供密码哈希和验证功能的帮助类
/// </summary>
public class PasswordHasherWithArgon2 : IPasswordHasher
{
	/// <summary>
	/// 生成指定大小的盐值
	/// </summary>
	/// <param name="size">指定大小</param>
	/// <returns>返回生成的盐值字节数组</returns>
	private static byte[] GenerateSalt(int size = 16)
	{
		var salt = new byte[size];
		// 使用加密随机数生成器生成盐值
		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(salt);
		}
		return salt;
	}

	/// <summary>
	/// 哈希密码并返回盐值和哈希值的组合字符串
	/// </summary>
	/// <param name="password">待hash的密码</param>
	/// <returns>返回盐值和哈希值的组合字符串</returns>
	public string HashPassword(string password)
	{
		// 生成盐值
		var salt = GenerateSalt();
		// 使用 Argon2id 算法进行哈希
		var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
		{
			Salt = salt, // 设置盐值
			DegreeOfParallelism = 8, // 指定在哈希过程中使用的并行线程数
			Iterations = 4, // 指定迭代次数
			MemorySize = 1024 * 1024 // 指定内存大小，1 GB
		};

		// 生成哈希值
		var hash = argon2.GetBytes(16);
		// 返回盐值和哈希值的组合字符串
		return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
	}

	/// <summary>
	/// 验证输入的密码是否与存储的哈希值匹配
	/// </summary>
	/// <param name="storedHash">存储的哈希值</param>
	/// <param name="password">输入的密码</param>
	/// <returns>如果匹配返回 true，否则返回 false</returns>
	public bool VerifyPassword(string storedHash, string password)
	{
		// 分割存储的哈希值字符串，获取盐值和哈希值
		var parts = storedHash.Split(':');
		if (parts.Length != 2)
			return false; // 如果格式不正确，返回 false

		var salt = Convert.FromBase64String(parts[0]);
		var hash = Convert.FromBase64String(parts[1]);

		// 使用 Argon2id 算法重新计算输入密码的哈希值
		var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
		{
			Salt = salt, // 设置盐值
			DegreeOfParallelism = 8, // 指定在哈希过程中使用的并行线程数
			Iterations = 4, // 指定迭代次数
			MemorySize = 1024 * 1024 // 指定内存大小，1 GB
		};

		var computedHash = argon2.GetBytes(16);
		// 比较计算出的哈希值和存储的哈希值是否相等
		return hash.SequenceEqual(computedHash);
	}
}
