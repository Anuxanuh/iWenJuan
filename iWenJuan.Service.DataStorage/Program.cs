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

		#region ���ݿ��������
		// ��� PostgreSQL ���ݿ�������
		builder.AddNpgsqlDbContext<StorageDbContext>("surveyDataStorageDb");
		#endregion ���ݿ��������

		#region ����ע���������
		// ���� HttpClient �����������û��������ͨ��
		builder.Services.AddHttpClient("SurveyService", client =>
		{
			// �������л�ȡ AuthService �Ķ˵� URL
			var url = builder.Configuration["Endpoint:SurveyService"]
					  ?? throw new InvalidOperationException("SurveyService endpoint is not configured.");
			client.BaseAddress = new Uri(url);
		});
		// ������ݵ������������ע������
		builder.Services.AddScoped<IExportService, ExportService>();
		// ����ļ��洢���������ע������
		builder.Services.AddScoped<IFileStorageService, FileStorageService>();
		#endregion ����ע���������

		var app = builder.Build();

		#region ���ݿ��Զ�Ǩ��
		using (var scope = app.Services.CreateScope())
		{
			var dbContext = scope.ServiceProvider.GetRequiredService<StorageDbContext>();
			var isCreated = dbContext.Database.EnsureCreated();
			//if (!isCreated)
			//	dbContext.Database.Migrate();
		}
		#endregion ���ݿ��Զ�Ǩ��

		app.MapDefaultEndpoints();

		if (app.Environment.IsDevelopment())
			app.MapOpenApi();

		app.UseHttpsRedirection();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}
