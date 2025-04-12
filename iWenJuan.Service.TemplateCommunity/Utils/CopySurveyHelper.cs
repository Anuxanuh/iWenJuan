using iWenJuan.Shared.Dtos;

namespace iWenJuan.Service.TemplateCommunity.Utils;

public static class CopySurveyHelper
{
	public static SurveyDto Copy(this SurveyDto survey)
	{
		return Copy(survey, survey.CreatedBy);
	}

	public static SurveyDto Copy(this SurveyDto survey, Guid newCreatedBy)
	{
		// 复制问卷的基本信息
		var newSurvey = new SurveyDto
		{
			Title = survey.Title,
			Description = survey.Description,
			IsPublished = false,
			CreatedBy = newCreatedBy,
			Questions = survey.Questions?.Select(q => q.Copy()).ToList()
		};
		return newSurvey;
	}

	public static QuestionDto Copy(this QuestionDto question)
	{
		// 复制问题的基本信息
		var newQuestion = new QuestionDto
		{
			QuestionType = question.QuestionType,
			QuestionText = question.QuestionText,
			Options = question.Options?.Select(o => o.Copy()).ToList(),
		};
		return newQuestion;
	}

	public static OptionDto Copy(this OptionDto option)
	{
		// 复制选项的基本信息
		var newOption = new OptionDto
		{
			OptionText = option.OptionText,
		};
		return newOption;
	}
}
