using iWenJuan.Service.Auth.Data;
using iWenJuan.Service.Auth.Interface;
using iWenJuan.Service.Auth.Models;
using iWenJuan.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace iWenJuan.Service.Auth.Controllers;

/// <summary>
/// 认证控制器，处理用户登录和注册请求
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly ILogger<AuthController> _logger;

	private readonly AuthDbContext _context;
	private readonly IPasswordHasher _passwordHasher;
	private readonly IJwtTokenHelper<User> _jwtTokenHelper;
	private readonly IVerificationCodeService _verificationCodeService;

	/// <summary>
	/// 构造函数，初始化依赖项
	/// </summary>
	/// <param name="context">数据库上下文</param>
	/// <param name="passwordHasher">密码哈希器</param>
	/// <param name="jwtTokenGenerator">JWT令牌生成器</param>
	/// <param name="verificationCodeService">验证码服务</param>
	public AuthController(AuthDbContext context, IPasswordHasher passwordHasher, IJwtTokenHelper<User> jwtTokenGenerator, IVerificationCodeService verificationCodeService, ILogger<AuthController> logger)
	{
		_context = context;
		_passwordHasher = passwordHasher;
		_jwtTokenHelper = jwtTokenGenerator;
		_verificationCodeService = verificationCodeService;
		_logger = logger;
	}

	/// <summary>
	/// 用户注册
	/// </summary>
	/// <param name="Email">注册邮箱</param>
	/// <returns>注册结果</returns>
	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] string Email)
	{
		_logger.LogInformation($"用户注册请求: {Email}", Email);

		// 检查用户名或邮箱是否已存在
		if (await _context.Users.AnyAsync(u => u.Email == Email))
			return BadRequest("该邮箱已注册");

		// 生成验证码并发送到用户邮箱
		await _verificationCodeService.GetAndSendVerificationCode(Email);

		return Ok("验证码已发送, 5分钟内有效");
	}

	/// <summary>
	/// 验证注册
	/// </summary>
	/// <param name="verifyRegisterDto">验证注册信息</param>
	/// <returns>验证结果</returns>
	[HttpPost("verify-register")]
	public async Task<IActionResult> VerifyRegister([FromBody] VerifyRegisterDto verifyRegisterDto)
	{
		// 验证验证码是否正确
		if (!await _verificationCodeService.VerifyVerificationCode(verifyRegisterDto.UserEmail, verifyRegisterDto.VerificationCode))
		{
			return BadRequest("验证码错误");
		}

		// 哈希用户密码并保存用户信息到数据库
		var user = new User()
		{
			UserName = verifyRegisterDto.UserName,
			PasswordHash = _passwordHasher.HashPassword(verifyRegisterDto.UserPassword),
			Email = verifyRegisterDto.UserEmail
		};
		_context.Users.Add(user);
		await _context.SaveChangesAsync();

		_logger.LogInformation($"用户{verifyRegisterDto.UserEmail}注册成功", verifyRegisterDto.UserEmail);

		return Ok("注册成功");
	}

	/// <summary>
	/// 用户登录
	/// </summary>
	/// <param name="loginDto">登录信息</param>
	/// <returns>登录结果</returns>
	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
	{
		// 查找用户是否存在
		var userInDb = await _context.Users
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefaultAsync(u => u.Email == loginDto.UserEmail);
		if (userInDb == null)
			return NotFound("该用户不存在");

		// 验证密码是否正确
		if (!_passwordHasher.VerifyPassword(userInDb.PasswordHash, loginDto.UserPassword))
			return Unauthorized("密码错误");

		// 生成JWT令牌
		var tokenString = _jwtTokenHelper.GenerateToken(userInDb);

		_logger.LogInformation($"用户{loginDto.UserEmail}登录请求", loginDto.UserEmail);

		return Ok(new LoginResponseDto { Token = tokenString, Message = "登录成功" });
	}

	/// <summary>
	/// 请求更改密码
	/// </summary>
	/// <param name="email">账号邮箱</param>
	/// <returns>请求结果</returns>
	[HttpPost("change-password")]
	public async Task<IActionResult> ChangePassword([FromBody] string email)
	{
		// 查找用户是否存在
		var userInDb = await _context.Users
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefaultAsync(u => u.Email == email);
		if (userInDb == null)
			return NotFound("该用户不存在");

		// 发送验证码到用户邮箱
		_verificationCodeService.GetAndSendVerificationCode(email);

		return Ok("验证码已发送, 5分钟内有效");
	}

	/// <summary>
	/// 验证更改密码
	/// </summary>
	/// <param name="verifyChangePasswordDto">验证更改密码信息</param>
	/// <returns>验证结果</returns>
	[HttpPost("verify-change-password")]
	public async Task<IActionResult> VerifyChangePassword([FromBody] VerifyChangePasswordDto verifyChangePasswordDto)
	{
		// 查找用户是否存在
		var userInDb = await _context.Users
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefaultAsync(u => u.Email == verifyChangePasswordDto.UserEmail);
		if (userInDb == null)
			return NotFound("该用户不存在");

		// 验证验证码是否正确
		if (!await _verificationCodeService.VerifyVerificationCode(userInDb.Email, verifyChangePasswordDto.VerificationCode))
			return BadRequest("验证码错误");

		// 更新用户密码并保存到数据库
		userInDb.PasswordHash = _passwordHasher.HashPassword(verifyChangePasswordDto.NewPassword);
		_context.Users.Update(userInDb);
		await _context.SaveChangesAsync();

		return Ok("密码修改成功");
	}

	/// <summary>
	/// 验证JWT令牌的有效性
	/// </summary>
	/// <param name="token">要验证的JWT令牌</param>
	/// <returns>验证结果</returns>
	[HttpPost("validate-token")]
	public IActionResult ValidateToken([FromBody] string token)
	{
		_logger.LogInformation($"Token: {token}", token);

		(var message, var userId) = _jwtTokenHelper.ValidateToken(token);
		if (message.Equals("验证成功"))
		{
			return Ok(userId);
		}
		else
		{
			return Unauthorized(message);
		}
	}
}
