using iWenJuan.Service.Survey.Extension;
using iWenJuan.Service.Survey.Interface;
using iWenJuan.Service.Survey.Services;
using iWenJuan.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iWenJuan.Service.Survey.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
	private readonly IDashboardService _dashboardService;

	public DashboardController(IDashboardService dashboardService)
	{
		_dashboardService = dashboardService;
	}


	[HttpGet("{userId}/all-surveys")]
	public async Task<IActionResult> GetAllSurveys(Guid userId)
	{
		// 获取所有问卷
		var surveys = await _dashboardService.GetAllServeys(userId);
		var responseSurveys = surveys.Select(s => s.ToDtoWithAnswers());
		// 返回包含所有问卷的Ok结果
		return Ok(responseSurveys);
	}
}
