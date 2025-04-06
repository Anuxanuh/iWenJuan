using iWenJuan.Service.Survey.Data;

namespace iWenJuan.Service.TemplateCommunity;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

		// Add services to the container.

		#region ���ݿ��������
		// ��� PostgreSQL ���ݿ�������
		builder.AddNpgsqlDbContext<SurveyDbContext>("surveyDataDb");
		#endregion ���ݿ��������

		builder.Services.AddHttpClient("SurveyService", client =>
		{
			// �������л�ȡ SurveyService �Ķ˵� URL
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
