using iWenJuan.Service.Auth.Interface;
using iWenJuan.Service.Auth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace iWenJuan.Service.Auth.Services;

/// <summary>
/// 用于生成JWT令牌的帮助类。
/// </summary>
public class JwtTokenHelper : IJwtTokenHelper<User>
{
	private readonly IConfiguration _configuration;

	public JwtTokenHelper(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	/// <summary>
	/// 为指定用户生成JWT令牌。
	/// </summary>
	/// <param name="user">生成令牌的用户。</param>
	/// <returns>生成的JWT令牌字符串。</returns>
	public string GenerateToken(User user)
	{
		// 创建一个JwtSecurityTokenHandler实例，用于生成JWT令牌
		var tokenHandler = new JwtSecurityTokenHandler();
		// 将密钥字符串转换为字节数组
		var keyBytes = Encoding.ASCII.GetBytes(
			_configuration["Jwt:Key"] ??
			throw new InvalidOperationException("Jwtsetting is not configured."));
		// 定义令牌的描述信息，包括声明、过期时间和签名凭证
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			// 设置令牌的声明信息
			Subject = new ClaimsIdentity([
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email)
			]),

			// 设置令牌的过期时间
			Expires = DateTime.UtcNow.AddMinutes(
				int.Parse(_configuration["Jwt:ExpireMinutes"] ??
				throw new InvalidOperationException("Jwtsetting is not configured."))),

			// 设置令牌的签名凭证，使用对称安全密钥和HmacSha256算法进行签名
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
		};
		// 创建JWT令牌
		var token = tokenHandler.CreateToken(tokenDescriptor);
		// 将令牌写为字符串并返回
		return tokenHandler.WriteToken(token);
	}

	/// <summary>
	/// 验证JWT令牌的有效性。
	/// </summary>
	/// <param name="token">要验证的JWT令牌。</param>
	/// <returns>如果令牌有效，返回用户id, 否则返回-1</returns>
	public (string, string) ValidateToken(string token)
	{
		// 创建一个JwtSecurityTokenHandler实例，用于验证JWT令牌
		var tokenHandler = new JwtSecurityTokenHandler();
		// 将密钥字符串转换为字节数组
		var keyBytes = Encoding.ASCII.GetBytes(
			_configuration["Jwt:Key"] ??
			throw new InvalidOperationException("Jwtsetting is not configured."));
		try
		{
			// 验证令牌
			var principal = tokenHandler.ValidateToken(token,
				new TokenValidationParameters
				{
					// 验证签名密钥
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
					// 是否验证令牌的发行者
					ValidateIssuer = false,
					// 是否验证令牌的受众
					ValidateAudience = false,
					// 设置时钟偏差为零
					ClockSkew = TimeSpan.Zero
				},
				out SecurityToken validatedToken);

			// 获取令牌中的用户ID声明
			var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			// 如果令牌中没有用户ID声明，返回验证失败
			if (userId == null)
				return ("验证失败", "");

			// 如果令牌验证成功，返回用户ID
			return ("验证成功", userId);
		}
		catch (SecurityTokenExpiredException)
		{
			// 令牌过期
			return ("令牌过期", "");
		}
		catch (SecurityTokenInvalidSignatureException)
		{
			// 签名无效
			return ("签名无效", "");
		}
		catch (Exception ex)
		{
			// 其他异常
			return ($"令牌验证失败: {ex.Message}", "");
		}
	}
}
