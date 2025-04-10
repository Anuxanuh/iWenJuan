using iWenJuan.Shared.Dtos;
using SurveyModel = iWenJuan.Service.Survey.Models.Survey;

namespace iWenJuan.Service.TemplateCommunity.Extension;

public static class SearchResultExtension
{
	public static SearchResultDto ToSearchResultDto(this SurveyModel survey)
	{
		return new SearchResultDto
		{
			Id = survey.SurveyId,
			Title = survey.Title,
			Description = survey.Description
		};
	}
}
