using iWenJuan.Service.Survey.Data;
using iWenJuan.Service.Survey.Interface;
using Microsoft.EntityFrameworkCore;

namespace iWenJuan.Service.Survey.Services;

public class DashboardService : IDashboardService
{
	private readonly SurveyDbContext _context;
	private readonly ILogger<SurveyService> _logger;

	public DashboardService(SurveyDbContext context, ILogger<SurveyService> logger)
	{
		_context = context;
		_logger = logger;
	}

	public async Task<List<Models.Survey>> GetAllServeys(Guid createdBy)
	{
		var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
		var surveys = await _context.Surveys.AsNoTrackingWithIdentityResolution()
										   .Include(s => s.Questions)
										   .ThenInclude(q => q.Answers)
										   .Where(s => s.CreatedBy == createdBy)
										   .ToListAsync();
		_logger.LogInformation("{createdBy} 获取了所有问卷信息 (包含问题, 答案)", createdBy);
		return surveys ?? [];
	}
}
