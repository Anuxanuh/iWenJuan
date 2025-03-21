using iWenJuan.Service.DataStorage.Data;
using iWenJuan.Service.DataStorage.Interface;
using iWenJuan.Service.DataStorage.Services;
using Microsoft.EntityFrameworkCore;

namespace iWenJuan.Service.DataStorage;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.AddServiceDefaults();


		builder.Services.AddControllers();
		// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
		builder.Services.AddOpenApi();

		#region 数据库服务配置
		// 添加 PostgreSQL 数据库上下文
		builder.AddNpgsqlDbContext<StorageDbContext>("surveyDataStorageDb");
		#endregion 数据库服务配置

		#region 依赖注入服务配置
		// 配置 HttpClient 服务，用于与用户服务进行通信
		builder.Services.AddHttpClient("SurveyService", client =>
		{
			// 从配置中获取 AuthService 的端点 URL
			var url = builder.Configuration["Endpoint:SurveyService"]
					  ?? throw new InvalidOperationException("SurveyService endpoint is not configured.");
			client.BaseAddress = new Uri(url);
		});
		// 添加数据导出服务的依赖注入配置
		builder.Services.AddScoped<IExportService, ExportService>();
		// 添加文件存储服务的依赖注入配置
		builder.Services.AddScoped<IFileStorageService, FileStorageService>();
		#endregion 依赖注入服务配置

		var app = builder.Build();

		#region 数据库自动迁移
		using (var scope = app.Services.CreateScope())
		{
			var dbContext = scope.ServiceProvider.GetRequiredService<StorageDbContext>();
			var isCreated = dbContext.Database.EnsureCreated();
			//if (!isCreated)
			//	dbContext.Database.Migrate();
		}
		#endregion 数据库自动迁移

		app.MapDefaultEndpoints();

		if (app.Environment.IsDevelopment())
			app.MapOpenApi();

		app.UseHttpsRedirection();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}
