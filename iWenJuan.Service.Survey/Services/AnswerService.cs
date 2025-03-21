using iWenJuan.Service.Survey.Data;
using iWenJuan.Service.Survey.Interface;
using iWenJuan.Service.Survey.Models;
using Microsoft.EntityFrameworkCore;

namespace iWenJuan.Service.Survey.Services;

public class AnswerService : IAnswerService
{
	private readonly SurveyDbContext _context;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="context">数据库上下文</param>
	public AnswerService(SurveyDbContext context)
	{
		_context = context;
	}

	/// <summary>
	/// 提交答案
	/// </summary>
	/// <param name="answer">答案对象</param>
	/// <returns>异步任务</returns>
	public async Task SubmitAnswerAsync(Answer answer)
	{
		_context.Answers.Add(answer);
		await _context.SaveChangesAsync();
	}

	/// <summary>
	/// 根据调查问卷ID获取答案
	/// </summary>
	/// <param name="surveyId">调查问卷ID</param>
	/// <returns>答案集合的异步任务</returns>
	public async Task<IEnumerable<Answer>> GetAnswersBySurveyIdAsync(int surveyId)
	{
		return await _context.Answers
			.Include(a => a.Question)
			.Where(a => a.Question.SurveyId == surveyId)
			.ToListAsync();
	}
}
