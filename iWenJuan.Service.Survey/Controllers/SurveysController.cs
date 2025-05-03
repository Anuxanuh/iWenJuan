using iWenJuan.Service.Survey.Data;
using iWenJuan.Service.Survey.Extension;
using iWenJuan.Service.Survey.Interface;
using iWenJuan.Service.Survey.Models;
using iWenJuan.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SurveyModel = iWenJuan.Service.Survey.Models.Survey;

namespace iWenJuan.Service.Survey.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SurveysController : ControllerBase
{
	private readonly ISurveyService _surveyService;
	private readonly ILogger<SurveysController> _logger;

	public SurveysController(ISurveyService surveyService, ILogger<SurveysController> logger)
	{
		this._surveyService = surveyService;
		_logger = logger;
	}

	/// <summary>
	/// 获取某个用户的所有问卷。
	/// 业务上用于获取某个用户创建的所有问卷的消息, 故不包含问卷的问题、选项和答案。
	/// </summary>
	/// <returns>包含所有问卷的ActionResult。</returns>
	[HttpGet("{userId}/all-surveys")]
	public async Task<IActionResult> GetAllSurveys(Guid userId)
	{
		// 获取所有问卷
		var surveys = await _surveyService.GetSurveysAsync(userId);
		var responseSurveys = surveys.Select(s => new SurveyDto
		{
			SurveyId = s.SurveyId,
			Title = s.Title,
			Description = s.Description,
			IsPublished = s.IsPublished,
			CreatedBy = s.CreatedBy,
			CreatedAt = s.CreatedAt,
		});
		// 返回包含所有问卷的Ok结果
		return Ok(responseSurveys);
	}

	/// <summary>
	/// 根据ID获取问卷。
	/// 包含问卷的问题、选项和逻辑。
	/// </summary>
	/// <param name="id">问卷ID。</param>
	/// <returns>包含问卷的ActionResult。</returns>
	[HttpGet("{id}")]
	public async Task<IActionResult> GetSurveyById(int id)
	{
		// 异步根据ID获取问卷
		var survey = await _surveyService.GetSurveyWithAllParamsAsync(id);
		// 如果问卷不存在，返回NotFound结果
		if (survey == null)
			return NotFound();
		var surveyDto = survey.ToDto();
		// 返回包含问卷的Ok结果
		return Ok(surveyDto);
	}

	/// <summary>
	/// 创建新问卷。
	/// </summary>
	/// <param name="survey">要创建的问卷。</param>
	/// <returns>包含创建结果的IActionResult。</returns>
	[HttpPost]
	public async Task<IActionResult> CreateSurvey([FromBody] SurveyDto survey)
	{
		// 创建问卷模型
		var surveyModel = survey.ToModelWithFullMapper();
		// 异步创建问卷
		var newSurvey = await _surveyService.CreateSurveyAsync(surveyModel);
		// 将创建的问卷转换为DTO
		var surveyDto = newSurvey.ToDto();
		// 返回包含创建问卷的CreatedAtAction结果
		return CreatedAtAction(nameof(GetSurveyById), new { id = surveyDto.SurveyId }, surveyDto);
	}

	/// <summary>
	/// 更新现有问卷。
	/// </summary>
	/// <param name="id">问卷ID。</param>
	/// <param name="survey">要更新的问卷。</param>
	/// <returns>包含更新结果的IActionResult。</returns>
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateSurvey(int id, [FromBody] SurveyDto survey)
	{
		// 记录更新问卷请求
		_logger.LogInformation("更新问卷请求: {id}", id);

		// 检查问卷ID是否匹配
		if (id != survey.SurveyId)
		{
			return BadRequest();
		}
		// 更新问卷模型
		var surveyModel = survey.ToModelWithFullMapper();
		// 异步更新问卷
		var newSurvey = await _surveyService.UpdateSurveyAsync(surveyModel);
		var surveyDto = newSurvey.ToDto();
		// 返回结果
		return CreatedAtAction(nameof(GetSurveyById), new { id = survey.SurveyId }, surveyDto);
	}

	/// <summary>
	/// 删除问卷。
	/// </summary>
	/// <param name="id">问卷ID。</param>
	/// <returns>包含删除结果的IActionResult。</returns>
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteSurvey(int id)
	{
		// 异步删除问卷
		await _surveyService.DeleteSurveyAsync(id);
		// 返回结果
		return NoContent();
	}
}
