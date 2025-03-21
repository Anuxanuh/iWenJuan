using iWenJuan.Service.Survey.Models;

namespace iWenJuan.Service.Survey.Interface;

public interface IAnswerService
{
	/// <summary>
	/// 提交答案
	/// </summary>
	/// <param name="answer">答案对象</param>
	/// <returns>异步任务</returns>
	Task SubmitAnswerAsync(Answer answer);
	/// <summary>
	/// 根据调查问卷ID获取答案
	/// </summary>
	/// <param name="surveyId">调查问卷ID</param>
	/// <returns>答案集合的异步任务</returns>
	Task<IEnumerable<Answer>> GetAnswersBySurveyIdAsync(int surveyId);
}
