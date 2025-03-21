using iWenJuan.Service.Auth.Interface;
using iWenJuan.Service.Auth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace iWenJuan.Service.Auth.Services;

/// <summary>
/// ��������JWT���Ƶİ����ࡣ
/// </summary>
public class JwtTokenHelper : IJwtTokenHelper<User>
{
	private readonly IConfiguration _configuration;

	public JwtTokenHelper(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	/// <summary>
	/// Ϊָ���û�����JWT���ơ�
	/// </summary>
	/// <param name="user">�������Ƶ��û���</param>
	/// <returns>���ɵ�JWT�����ַ�����</returns>
	public string GenerateToken(User user)
	{
		// ����һ��JwtSecurityTokenHandlerʵ������������JWT����
		var tokenHandler = new JwtSecurityTokenHandler();
		// ����Կ�ַ���ת��Ϊ�ֽ�����
		var keyBytes = Encoding.ASCII.GetBytes(
			_configuration["Jwt:Key"] ??
			throw new InvalidOperationException("Jwtsetting is not configured."));
		// �������Ƶ�������Ϣ����������������ʱ���ǩ��ƾ֤
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			// �������Ƶ�������Ϣ
			Subject = new ClaimsIdentity([
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email)
			]),

			// �������ƵĹ���ʱ��
			Expires = DateTime.UtcNow.AddMinutes(
				int.Parse(_configuration["Jwt:ExpireMinutes"] ??
				throw new InvalidOperationException("Jwtsetting is not configured."))),

			// �������Ƶ�ǩ��ƾ֤��ʹ�öԳư�ȫ��Կ��HmacSha256�㷨����ǩ��
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
		};
		// ����JWT����
		var token = tokenHandler.CreateToken(tokenDescriptor);
		// ������дΪ�ַ���������
		return tokenHandler.WriteToken(token);
	}

	/// <summary>
	/// ��֤JWT���Ƶ���Ч�ԡ�
	/// </summary>
	/// <param name="token">Ҫ��֤��JWT���ơ�</param>
	/// <returns>���������Ч�������û�id, ���򷵻�-1</returns>
	public (string, string) ValidateToken(string token)
	{
		// ����һ��JwtSecurityTokenHandlerʵ����������֤JWT����
		var tokenHandler = new JwtSecurityTokenHandler();
		// ����Կ�ַ���ת��Ϊ�ֽ�����
		var keyBytes = Encoding.ASCII.GetBytes(
			_configuration["Jwt:Key"] ??
			throw new InvalidOperationException("Jwtsetting is not configured."));
		try
		{
			// ��֤����
			var principal = tokenHandler.ValidateToken(token,
				new TokenValidationParameters
				{
					// ��֤ǩ����Կ
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
					// �Ƿ���֤���Ƶķ�����
					ValidateIssuer = false,
					// �Ƿ���֤���Ƶ�����
					ValidateAudience = false,
					// ����ʱ��ƫ��Ϊ��
					ClockSkew = TimeSpan.Zero
				},
				out SecurityToken validatedToken);

			// ��ȡ�����е��û�ID����
			var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			// ���������û���û�ID������������֤ʧ��
			if (userId == null)
				return ("��֤ʧ��", "");

			// ���������֤�ɹ��������û�ID
			return ("��֤�ɹ�", userId);
		}
		catch (SecurityTokenExpiredException)
		{
			// ���ƹ���
			return ("���ƹ���", "");
		}
		catch (SecurityTokenInvalidSignatureException)
		{
			// ǩ����Ч
			return ("ǩ����Ч", "");
		}
		catch (Exception ex)
		{
			// �����쳣
			return ($"������֤ʧ��: {ex.Message}", "");
		}
	}
}
