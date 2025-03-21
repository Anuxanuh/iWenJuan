using iWenJuan.Service.Auth.Data;
using iWenJuan.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iWenJuan.Service.Auth.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	private readonly AuthDbContext _context;

	public UserController(AuthDbContext context)
	{
		_context = context;
	}

	/// <summary>
	/// 根据用户ID获取用户信息
	/// </summary>
	/// <param name="userId">用户ID</param>
	/// <returns>用户信息</returns>
	[HttpGet("{userId}")]
	public async Task<IActionResult> GetUserById(Guid userId)
	{
		// 查找用户是否存在
		var userInDb = await _context.Users
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefaultAsync(u => u.Id.Equals(userId));
		// 如果用户不存在，返回404
		if (userInDb == null)
			return NotFound("该用户不存在");

		// 返回用户信息
		return Ok(new UserDto()
		{
			Id = userInDb.Id,
			UserName = userInDb.UserName,
			Email = userInDb.Email,
			CreatedAt = userInDb.CreatedAt
		});
	}
}
