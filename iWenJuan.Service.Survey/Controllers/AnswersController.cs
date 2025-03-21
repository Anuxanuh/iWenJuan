using iWenJuan.Service.Survey.Interface;
using iWenJuan.Service.Survey.Models;
using iWenJuan.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace iWenJuan.Service.Survey.Controllers;

/// <summary>
/// 答案控制器，处理与答案相关的API请求
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AnswersController : ControllerBase
{
	private readonly IAnswerService _answerService;

	/// <summary>
	/// 构造函数，初始化答案服务
	/// </summary>
	/// <param name="answerService">答案服务接口</param>
	public AnswersController(IAnswerService answerService)
	{
		_answerService = answerService;
	}

	/// <summary>
	/// 提交答案的API端点
	/// </summary>
	/// <param name="answers">答案列表</param>
	/// <returns>操作结果</returns>
	[HttpPost]
	public async Task<IActionResult> SubmitAnswers([FromBody] IEnumerable<AnswerDto> answers)
	{
		// 创建一个新的用户ID
		var newUserId = Guid.NewGuid();
		// 提交答题时间
		//var time = DateTime.UtcNow;

		// 遍历每个答案并提交
		foreach (var answer in answers)
		{
			await _answerService.SubmitAnswerAsync(new Answer
			{
				UserId = newUserId,
				QuestionId = answer.QuestionId,
				AnswerText = answer.AnswerText,
				CreatedAt = answer.CreatedAt,
			});
		}

		// 返回操作成功的响应
		return Ok();
	}
}
