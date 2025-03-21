using iWenJuan.Service.Survey.Data;
using iWenJuan.Service.Survey.Interface;
using iWenJuan.Service.Survey.Models;
using Microsoft.EntityFrameworkCore;
using SurveyModel = iWenJuan.Service.Survey.Models.Survey;

namespace iWenJuan.Service.Survey.Services;

public class SurveyService : ISurveyService
{
	private readonly SurveyDbContext _context;
	private readonly ILogger<SurveyService> _logger;

	public SurveyService(SurveyDbContext context, ILogger<SurveyService> logger)
	{
		_context = context;
		_logger = logger;
	}

	#region 问卷
	public async Task<IEnumerable<SurveyModel>> GetSurveysAsync(Guid createdBy)
	{
		_logger.LogInformation("获取用户 {createdBy} 所有的问卷", createdBy);
		return await _context.Surveys.AsNoTracking()
									 .Where(s => s.CreatedBy == createdBy)
									 .ToListAsync();
	}
	public async Task<SurveyModel> GetSurveyAsync(int surveyId)
	{
		var survey = await _context.Surveys.AsNoTracking()
										   .FirstOrDefaultAsync(s => s.SurveyId == surveyId);
		if (survey == null)
		{
			_logger.LogWarning("获取问卷 {surveyId} 时未找到", surveyId);
			return default!;
		}
		_logger.LogInformation("获取了问卷 {surveyId}", surveyId);
		return survey;
	}
	public async Task<SurveyModel> GetSurveyWithAllParamsAsync(int surveyId)
	{
		var survey = await _context.Surveys.AsNoTrackingWithIdentityResolution()
										   .Include(s => s.Questions)
										   .ThenInclude(q => q.Options)
										   .Include(s => s.Questions)
										   .ThenInclude(q => q.Conditions)
										   .FirstOrDefaultAsync(s => s.SurveyId == surveyId);
		if (survey == null)
		{
			_logger.LogWarning("获取问卷 {surveyId} 时未找到", surveyId);
			return default!;
		}
		_logger.LogInformation("获取了问卷 {surveyId} (包含问题, 选项, 逻辑)", surveyId);
		return survey;
	}
	public async Task<SurveyModel> CreateSurveyAsync(SurveyModel survey)
	{
		_context.Surveys.Add(survey);
		await _context.SaveChangesAsync();
		_logger.LogInformation("创建了新问卷 {surveyId}", survey.SurveyId);
		return survey;
	}
	public async Task<SurveyModel> UpdateSurveyAsync(SurveyModel survey)
	{
		_context.Surveys.Update(survey);
		await _context.SaveChangesAsync();
		_logger.LogInformation("更新了问卷 {surveyId}", survey.SurveyId);
		return survey;
	}
	public async Task DeleteSurveyAsync(int surveyId)
	{
		var survey = await _context.Surveys.FindAsync(surveyId);
		if (survey != null)
		{
			_context.Surveys.Remove(survey);
			await _context.SaveChangesAsync();
			_logger.LogInformation("删除了问卷 {surveyId}", surveyId);
		}
		else
		{
			_logger.LogWarning("删除问卷 {surveyId} 时未找到", surveyId);
		}
	}
	#endregion 问卷

	#region 问题
	public async Task<IEnumerable<Question>> GetQuestionsAsync(int surveyId)
	{
		_logger.LogInformation("获取问卷 {surveyId} 的所有问题", surveyId);
		return await _context.Questions.AsNoTracking()
										.Where(q => q.SurveyId == surveyId)
										.ToListAsync();
	}

	public async Task<Question> GetQuestionAsync(int surveyId, int questionId)
	{
		var question = await _context.Questions.AsNoTracking()
												.FirstOrDefaultAsync(q => q.SurveyId == surveyId && q.QuestionId == questionId);
		if (question == null)
		{
			_logger.LogWarning("获取问卷 {surveyId} 的问题 {questionId} 时未找到", surveyId, questionId);
			return default!;
		}
		_logger.LogInformation("获取了问卷 {surveyId} 的问题 {questionId}", surveyId, questionId);
		return question;
	}

	public async Task<Question> GetQuestionWithAllParamsAsync(int surveyId, int questionId)
	{
		var question = await _context.Questions.AsNoTrackingWithIdentityResolution()
												.Include(q => q.Options)
												.Include(q => q.Conditions)
												.FirstOrDefaultAsync(q => q.SurveyId == surveyId && q.QuestionId == questionId);
		if (question == null)
		{
			_logger.LogWarning("获取问卷 {surveyId} 的问题 {questionId} 时未找到", surveyId, questionId);
			return default!;
		}
		_logger.LogInformation("获取了问卷 {surveyId} 的问题 {questionId} (包含选项, 逻辑)", surveyId, questionId);
		return question;
	}

	public async Task<Question> CreateQuestionAsync(int surveyId, Question question)
	{
		question.SurveyId = surveyId;
		_context.Questions.Add(question);
		await _context.SaveChangesAsync();
		_logger.LogInformation("为问卷 {surveyId} 创建了新问题 {questionId}", surveyId, question.QuestionId);
		return question;
	}

	public async Task<Question> UpdateQuestionAsync(int surveyId, Question question)
	{
		var existingQuestion = await _context.Questions.FirstOrDefaultAsync(q => q.SurveyId == surveyId && q.QuestionId == question.QuestionId);
		if (existingQuestion == null)
		{
			_logger.LogWarning("更新问卷 {surveyId} 的问题 {questionId} 时未找到", surveyId, question.QuestionId);
			return default!;
		}
		_context.Entry(existingQuestion).CurrentValues.SetValues(question);
		await _context.SaveChangesAsync();
		_logger.LogInformation("更新了问卷 {surveyId} 的问题 {questionId}", surveyId, question.QuestionId);
		return question;
	}

	public async Task DeleteQuestionAsync(int surveyId, int questionId)
	{
		var question = await _context.Questions.FirstOrDefaultAsync(q => q.SurveyId == surveyId && q.QuestionId == questionId);
		if (question != null)
		{
			_context.Questions.Remove(question);
			await _context.SaveChangesAsync();
			_logger.LogInformation("删除了问卷 {surveyId} 的问题 {questionId}", surveyId, questionId);
		}
		else
		{
			_logger.LogWarning("删除问卷 {surveyId} 的问题 {questionId} 时未找到", surveyId, questionId);
		}
	}
	#endregion 问题

	#region 选项
	public async Task<IEnumerable<Option>> GetOptionsAsync(int questionId)
	{
		_logger.LogInformation("获取问题 {questionId} 的所有选项", questionId);
		return await _context.Options.Where(o => o.QuestionId == questionId).ToListAsync();
	}

	public async Task<Option> GetOptionAsync(int questionId, int optionId)
	{
		var option = await _context.Options.FirstOrDefaultAsync(o => o.QuestionId == questionId && o.OptionId == optionId);
		if (option == null)
		{
			_logger.LogWarning("获取问题 {questionId} 的选项 {optionId} 时未找到", questionId, optionId);
			return default!;
		}
		_logger.LogInformation("获取了问题 {questionId} 的选项 {optionId}", questionId, optionId);
		return option;
	}

	public async Task<Option> CreateOptionAsync(int questionId, Option option)
	{
		option.QuestionId = questionId;
		_context.Options.Add(option);
		await _context.SaveChangesAsync();
		_logger.LogInformation("为问题 {questionId} 创建了新选项 {optionId}", questionId, option.OptionId);
		return option;
	}

	public async Task<Option> UpdateOptionAsync(int questionId, Option option)
	{
		var existingOption = await _context.Options.FirstOrDefaultAsync(o => o.QuestionId == questionId && o.OptionId == option.OptionId);
		if (existingOption == null)
		{
			_logger.LogWarning("更新问题 {questionId} 的选项 {optionId} 时未找到", questionId, option.OptionId);
			return default!;
		}
		_context.Entry(existingOption).CurrentValues.SetValues(option);
		await _context.SaveChangesAsync();
		_logger.LogInformation("更新了问题 {questionId} 的选项 {optionId}", questionId, option.OptionId);
		return option;
	}

	public async Task DeleteOptionAsync(int questionId, int optionId)
	{
		var option = await _context.Options.FirstOrDefaultAsync(o => o.QuestionId == questionId && o.OptionId == optionId);
		if (option != null)
		{
			_context.Options.Remove(option);
			await _context.SaveChangesAsync();
			_logger.LogInformation("删除了问题 {questionId} 的选项 {optionId}", questionId, optionId);
		}
		else
		{
			_logger.LogWarning("删除问题 {questionId} 的选项 {optionId} 时未找到", questionId, optionId);
		}
	}
	#endregion 选项

	#region 条件
	public async Task<IEnumerable<Condition>> GetConditionsAsync(int questionId)
	{
		_logger.LogInformation("获取问题 {questionId} 的所有条件", questionId);
		return await _context.Conditions.Where(c => c.QuestionId == questionId).ToListAsync();
	}

	public async Task<Condition> GetConditionAsync(int questionId, int conditionId)
	{
		var condition = await _context.Conditions.FirstOrDefaultAsync(c => c.QuestionId == questionId && c.ConditionId == conditionId);
		if (condition == null)
		{
			_logger.LogWarning("获取问题 {questionId} 的条件 {conditionId} 时未找到", questionId, conditionId);
			return default!;
		}
		_logger.LogInformation("获取了问题 {questionId} 的条件 {conditionId}", questionId, conditionId);
		return condition;
	}

	public async Task<Condition> CreateConditionAsync(int questionId, Condition condition)
	{
		condition.QuestionId = questionId;
		_context.Conditions.Add(condition);
		await _context.SaveChangesAsync();
		_logger.LogInformation("为问题 {questionId} 创建了新条件 {conditionId}", questionId, condition.ConditionId);
		return condition;
	}

	public async Task<Condition> UpdateConditionAsync(int questionId, Condition condition)
	{
		var existingCondition = await _context.Conditions.FirstOrDefaultAsync(c => c.QuestionId == questionId && c.ConditionId == condition.ConditionId);
		if (existingCondition == null)
		{
			_logger.LogWarning("更新问题 {questionId} 的条件 {conditionId} 时未找到", questionId, condition.ConditionId);
			return default!;
		}
		_context.Entry(existingCondition).CurrentValues.SetValues(condition);
		await _context.SaveChangesAsync();
		_logger.LogInformation("更新了问题 {questionId} 的条件 {conditionId}", questionId, condition.ConditionId);
		return condition;
	}

	public async Task DeleteConditionAsync(int conditionId)
	{
		var condition = await _context.Conditions.FirstOrDefaultAsync(c => c.ConditionId == conditionId);
		if (condition != null)
		{
			_context.Conditions.Remove(condition);
			await _context.SaveChangesAsync();
			_logger.LogInformation("删除了条件 {conditionId}", conditionId);
		}
		else
		{
			_logger.LogWarning("删除条件 {conditionId} 时未找到", conditionId);
		}
	}
	#endregion 条件
}
