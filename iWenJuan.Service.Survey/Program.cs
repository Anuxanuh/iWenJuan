using iWenJuan.Service.Survey.Data;
using iWenJuan.Service.Survey.Interface;
using iWenJuan.Service.Survey.Services;
using Microsoft.EntityFrameworkCore;

namespace iWenJuan.Service.Survey;

public class Program
{
	public static void Main(string[] args)
	{
		// 创建 WebApplicationBuilder 实例
		var builder = WebApplication.CreateBuilder(args);
		// 添加默认服务配置
		builder.AddServiceDefaults();

		// 向容器中添加服务

		// 添加控制器服务
		builder.Services.AddControllers();
		// 配置 OpenAPI 以生成 API 文档
		// 了解更多关于配置 OpenAPI 的信息，请访问 https://aka.ms/aspnet/openapi
		builder.Services.AddOpenApi();

		builder.Services.AddLogging();

		#region 数据库服务配置
		// 添加 PostgreSQL 数据库上下文
		builder.AddNpgsqlDbContext<SurveyDbContext>("surveyDataDb");
		#endregion 数据库服务配置

		#region 依赖注入服务配置
		// 添加问卷服务的依赖注入配置
		builder.Services.AddScoped<ISurveyService, SurveyService>();
		// 添加回答服务的依赖注入配置
		builder.Services.AddScoped<IAnswerService, AnswerService>();
		// 添加导出服务的依赖注入配置
		builder.Services.AddScoped<IExportService, ExportService>();
		// 添加仪表板服务的依赖注入配置
		builder.Services.AddScoped<IDashboardService, DashboardService>();
		#endregion 依赖注入服务配置

		// 构建 WebApplication 实例
		var app = builder.Build();

		#region 数据库自动迁移
		using (var scope = app.Services.CreateScope())
		{
			var dbContext = scope.ServiceProvider.GetRequiredService<SurveyDbContext>();
			var isCreated = dbContext.Database.EnsureCreated();
			//if (!isCreated)
			//	dbContext.Database.Migrate();
		}
		#endregion 数据库自动迁移

		// 映射默认端点
		app.MapDefaultEndpoints();

		// 在开发环境中映射 OpenAPI 端点
		if (app.Environment.IsDevelopment())
			app.MapOpenApi();

		// 启用 HTTPS 重定向
		app.UseHttpsRedirection();

		// 启用授权中间件
		app.UseAuthorization();

		// 映射控制器端点
		app.MapControllers();

		// 运行应用程序
		app.Run();
	}
}
