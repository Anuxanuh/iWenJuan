using iWenJuan.Service.Survey.Data;
using iWenJuan.Service.Survey.Interface;
using iWenJuan.Service.Survey.Services;
using Microsoft.EntityFrameworkCore;

namespace iWenJuan.Service.Survey;

public class Program
{
	public static void Main(string[] args)
	{
		// ���� WebApplicationBuilder ʵ��
		var builder = WebApplication.CreateBuilder(args);
		// ���Ĭ�Ϸ�������
		builder.AddServiceDefaults();

		// ����������ӷ���

		// ��ӿ���������
		builder.Services.AddControllers();
		// ���� OpenAPI ������ API �ĵ�
		// �˽����������� OpenAPI ����Ϣ������� https://aka.ms/aspnet/openapi
		builder.Services.AddOpenApi();

		builder.Services.AddLogging();

		#region ���ݿ��������
		// ��� PostgreSQL ���ݿ�������
		builder.AddNpgsqlDbContext<SurveyDbContext>("surveyDataDb");
		#endregion ���ݿ��������

		#region ����ע���������
		// ����ʾ���������ע������
		builder.Services.AddScoped<ISurveyService, SurveyService>();
		// ��ӻش���������ע������
		builder.Services.AddScoped<IAnswerService, AnswerService>();
		// ��ӵ������������ע������
		builder.Services.AddScoped<IExportService, ExportService>();
		// ����Ǳ����������ע������
		builder.Services.AddScoped<IDashboardService, DashboardService>();
		#endregion ����ע���������

		// ���� WebApplication ʵ��
		var app = builder.Build();

		#region ���ݿ��Զ�Ǩ��
		using (var scope = app.Services.CreateScope())
		{
			var dbContext = scope.ServiceProvider.GetRequiredService<SurveyDbContext>();
			var isCreated = dbContext.Database.EnsureCreated();
			//if (!isCreated)
			//	dbContext.Database.Migrate();
		}
		#endregion ���ݿ��Զ�Ǩ��

		// ӳ��Ĭ�϶˵�
		app.MapDefaultEndpoints();

		// �ڿ���������ӳ�� OpenAPI �˵�
		if (app.Environment.IsDevelopment())
			app.MapOpenApi();

		// ���� HTTPS �ض���
		app.UseHttpsRedirection();

		// ������Ȩ�м��
		app.UseAuthorization();

		// ӳ��������˵�
		app.MapControllers();

		// ����Ӧ�ó���
		app.Run();
	}
}
