namespace iWenJuan.Service.Survey.Interface;
using SurveyModel = iWenJuan.Service.Survey.Models.Survey;

public interface IDashboardService
{
	Task<List<SurveyModel>> GetAllServeys(Guid createdBy);
}
