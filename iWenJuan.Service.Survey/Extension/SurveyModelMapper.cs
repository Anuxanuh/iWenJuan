using iWenJuan.Service.Survey.Models;
using iWenJuan.Shared.Dtos;
using SurveyModel = iWenJuan.Service.Survey.Models.Survey;

namespace iWenJuan.Service.Survey.Extension;

public static class SurveyModelMapper
{
	public static SurveyDto ToDto(this SurveyModel survey)
	{
		return new SurveyDto
		{
			SurveyId = survey.SurveyId,
			Title = survey.Title,
			Description = survey.Description,
			IsPublished = survey.IsPublished,
			CreatedBy = survey.CreatedBy,
			CreatedAt = survey.CreatedAt,
			Questions = survey.Questions != null ?
			[.. survey.Questions.Select(q => new QuestionDto
			{
				QuestionId = q.QuestionId,
				QuestionType = q.QuestionType,
				QuestionText = q.QuestionText,
				Options = q.Options != null ?
				[.. q.Options.Select(o => new OptionDto
				{
					OptionId = o.OptionId,
					OptionText = o.OptionText,
				})]: default!,
				Conditions = q.Conditions != null ?
				[.. q.Conditions.Select(c => new ConditionDto
				{
					ConditionId = c.ConditionId,
					Operator = c.Operator,
					Value = c.Value,
					QuestionId = c.QuestionId,
					NextQuestionId = c.NextQuestionId,
				})] : default!,
				Answers = default!,
			})] : default!,
		};
	}

	public static SurveyDto ToDtoWithAnswers(this SurveyModel survey)
	{
		return new SurveyDto
		{
			SurveyId = survey.SurveyId,
			Title = survey.Title,
			Description = survey.Description,
			IsPublished = survey.IsPublished,
			CreatedBy = survey.CreatedBy,
			CreatedAt = survey.CreatedAt,
			Questions = survey.Questions != null ?
			[.. survey.Questions.Select(q => new QuestionDto
			{
				QuestionId = q.QuestionId,
				QuestionType = q.QuestionType,
				QuestionText = q.QuestionText,
				Options = default!,
				Conditions =  default!,
				Answers = q.Answers != null ?
				[.. q.Answers.Select(c => new AnswerDto
				{
					AnswerId = c.AnswerId,
					QuestionId = c.QuestionId,
					UserId = c.UserId,
					AnswerText = c.AnswerText,
					CreatedAt = c.CreatedAt,
				})] : default!,
			})] : default!,
		};
	}

	public static SurveyModel ToModelWithFullMapper(this SurveyDto survey)
	{
		return new SurveyModel
		{
			SurveyId = survey.SurveyId,
			Title = survey.Title,
			Description = survey.Description,
			IsPublished = survey.IsPublished,
			CreatedBy = survey.CreatedBy,
			CreatedAt = survey.CreatedAt,
			Questions = survey.Questions != null ?
			[.. survey.Questions.Select(q => new Question
			{
				QuestionId = q.QuestionId,
				QuestionType = q.QuestionType,
				QuestionText = q.QuestionText,
				Options = q.Options != null ?
				[.. q.Options.Select(o => new Option
				{
					OptionId = o.OptionId,
					OptionText = o.OptionText,
				})]: default!,
				Conditions = q.Conditions != null ?
				[.. q.Conditions.Select(c => new Condition
				{
					ConditionId = c.ConditionId,
					Operator = c.Operator,
					Value = c.Value,
					QuestionId = c.QuestionId,
					NextQuestionId = c.NextQuestionId,
				})] : default!
			})] : default!,
		};
	}
}
