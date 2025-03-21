using iWenJuan.Service.Survey.Models;
using SurveyModel = iWenJuan.Service.Survey.Models.Survey;

namespace iWenJuan.Service.Survey.Interface;

/// <summary>
/// 调查服务接口
/// </summary>
public interface ISurveyService
{
	#region 问卷
	/// <summary>
	/// 获取指定创建者的所有调查问卷。
	/// </summary>
	/// <param name="createdBy">创建者ID。</param>
	/// <returns>调查问卷集合。</returns>
	Task<IEnumerable<SurveyModel>> GetSurveysAsync(Guid createdBy);

	/// <summary>
	/// 根据ID获取特定调查问卷。
	/// </summary>
	/// <param name="surveyId">调查问卷ID。</param>
	/// <returns>指定ID的调查问卷。</returns>
	Task<SurveyModel> GetSurveyAsync(int surveyId);

	/// <summary>
	/// 根据ID获取特定调查问卷。
	/// 包含问题、选项和条件。
	/// </summary>
	/// <param name="surveyId">调查问卷ID。</param>
	/// <returns>指定ID的调查问卷。</returns>
	Task<SurveyModel> GetSurveyWithAllParamsAsync(int surveyId);

	/// <summary>
	/// 创建新的调查问卷。
	/// </summary>
	/// <param name="survey">要创建的调查问卷。</param>
	/// <returns>创建的调查问卷。</returns>
	Task<SurveyModel> CreateSurveyAsync(SurveyModel survey);

	/// <summary>
	/// 更新现有的调查问卷。
	/// </summary>
	/// <param name="survey">要更新的调查问卷。</param>
	/// <returns>更新后的调查问卷。</returns>
	Task<SurveyModel> UpdateSurveyAsync(SurveyModel survey);

	/// <summary>
	/// 根据ID删除调查问卷。
	/// </summary>
	/// <param name="surveyId">要删除的调查问卷ID。</param>
	Task DeleteSurveyAsync(int surveyId);
	#endregion 问卷

	#region 问题
	/// <summary>
	/// 获取特定调查问卷的所有问题。
	/// </summary>
	/// <param name="surveyId">调查问卷ID。</param>
	/// <returns>问题集合。</returns>
	Task<IEnumerable<Question>> GetQuestionsAsync(int surveyId);

	/// <summary>
	/// 根据ID获取特定问题。
	/// </summary>
	/// <param name="surveyId">调查问卷ID。</param>
	/// <param name="questionId">问题ID。</param>
	/// <returns>指定ID的问题。</returns>
	Task<Question> GetQuestionAsync(int surveyId, int questionId);

	/// <summary>
	/// 根据ID获取特定问题。
	/// 包含选项和条件。
	/// </summary>
	/// <param name="surveyId">调查问卷ID。</param>
	/// <param name="questionId">问题ID。</param>
	/// <returns>指定ID的问题。</returns>
	Task<Question> GetQuestionWithAllParamsAsync(int surveyId, int questionId);

	/// <summary>
	/// 为特定调查问卷创建新问题。
	/// </summary>
	/// <param name="surveyId">调查问卷ID。</param>
	/// <param name="question">要创建的问题。</param>
	/// <returns>创建的问题。</returns>
	Task<Question> CreateQuestionAsync(int surveyId, Question question);

	/// <summary>
	/// 更新特定调查问卷的现有问题。
	/// </summary>
	/// <param name="surveyId">调查问卷ID。</param>
	/// <param name="question">要更新的问题。</param>
	/// <returns>更新的问题。</returns>
	Task<Question> UpdateQuestionAsync(int surveyId, Question question);

	/// <summary>
	/// 根据ID删除问题。
	/// </summary>
	/// <param name="surveyId">调查问卷ID。</param>
	/// <param name="questionId">要删除的问题ID。</param>
	Task DeleteQuestionAsync(int surveyId, int questionId);
	#endregion 问题

	#region 选项
	/// <summary>
	/// 获取特定问题的所有选项。
	/// </summary>
	/// <param name="questionId">问题ID。</param>
	/// <returns>选项集合。</returns>
	Task<IEnumerable<Option>> GetOptionsAsync(int questionId);

	/// <summary>
	/// 根据ID获取特定选项。
	/// </summary>
	/// <param name="questionId">问题ID。</param>
	/// <param name="optionId">选项ID。</param>
	/// <returns>指定ID的选项。</returns>
	Task<Option> GetOptionAsync(int questionId, int optionId);

	/// <summary>
	/// 为特定问题创建新选项。
	/// </summary>
	/// <param name="questionId">问题ID。</param>
	/// <param name="option">要创建的选项。</param>
	/// <returns>创建的选项。</returns>
	Task<Option> CreateOptionAsync(int questionId, Option option);

	/// <summary>
	/// 更新特定问题的现有选项。
	/// </summary>
	/// <param name="questionId">问题ID。</param>
	/// <param name="option">要更新的选项。</param>
	/// <returns>更新的选项。</returns>
	Task<Option> UpdateOptionAsync(int questionId, Option option);

	/// <summary>
	/// 根据ID删除选项。
	/// </summary>
	/// <param name="questionId">问题ID。</param>
	/// <param name="optionId">要删除的选项ID。</param>
	Task DeleteOptionAsync(int questionId, int optionId);

	#endregion 选项

	#region 条件
	/// <summary>
	/// 获取特定问题的所有条件。
	/// </summary>
	/// <param name="questionId">问题ID。</param>
	/// <returns>条件集合。</returns>
	Task<IEnumerable<Condition>> GetConditionsAsync(int questionId);

	/// <summary>
	/// 根据ID获取特定条件。
	/// </summary>
	/// <param name="questionId">问题ID。</param>
	/// <param name="conditionId">条件ID。</param>
	/// <returns>指定ID的条件。</returns>
	Task<Condition> GetConditionAsync(int questionId, int conditionId);

	/// <summary>
	/// 为特定问题创建新条件。
	/// </summary>
	/// <param name="questionId">问题ID。</param>
	/// <param name="condition">要创建的条件。</param>
	/// <returns>创建的条件。</returns>
	Task<Condition> CreateConditionAsync(int questionId, Condition condition);

	/// <summary>
	/// 更新特定问题的现有条件。
	/// </summary>
	/// <param name="questionId">问题ID。</param>
	/// <param name="condition">要更新的条件。</param>
	/// <returns>更新的条件。</returns>
	Task<Condition> UpdateConditionAsync(int questionId, Condition condition);

	/// <summary>
	/// 根据ID删除条件。
	/// </summary>
	/// <param name="conditionId">要删除的条件ID。</param>
	Task DeleteConditionAsync(int conditionId);
	#endregion 条件
}
