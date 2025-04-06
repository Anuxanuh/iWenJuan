using iWenJuan.Service.Survey.Data;

namespace iWenJuan.Service.TemplateCommunity;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

		// Add services to the container.

		#region 数据库服务配置
		// 添加 PostgreSQL 数据库上下文
		builder.AddNpgsqlDbContext<SurveyDbContext>("surveyDataDb");
		#endregion 数据库服务配置

		builder.Services.AddHttpClient("SurveyService", client =>
		{
			// 从配置中获取 SurveyService 的端点 URL
			var url = builder.Configuration["Endpoint:SurveyService"]
					  ?? throw new InvalidOperationException("SurveyService endpoint is not configured.");
			client.BaseAddress = new Uri(url);
		});

		builder.Services.AddControllers();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
