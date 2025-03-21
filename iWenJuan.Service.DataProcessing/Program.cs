using iWenJuan.Service.DataProcessing.Interface;
using iWenJuan.Service.DataProcessing.Services;

namespace iWenJuan.Service.DataProcessing;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.AddServiceDefaults();

		// Add services to the container.

		builder.Services.AddControllers();
		// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
		builder.Services.AddOpenApi();

		#region 依赖注入服务配置
		builder.Services.AddHttpClient("DataStoreService", client =>
		{
			// 从配置中获取 AuthService 的端点 URL
			var url = builder.Configuration["Endpoint:DataStoreService"]
					  ?? throw new InvalidOperationException("DataStoreService endpoint is not configured.");
			client.BaseAddress = new Uri(url);
		});
		builder.Services.AddScoped<ICsvProcessingService, CsvProcessingService>();
		#endregion 依赖注入服务配置

		var app = builder.Build();

		app.MapDefaultEndpoints();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
			app.MapOpenApi();

		app.UseAuthorization();


		app.MapControllers();

		app.Run();
	}
}
