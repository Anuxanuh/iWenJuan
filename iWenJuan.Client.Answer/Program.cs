using iWenJuan.Client.Answer.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace iWenJuan.Client.Answer;
public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.AddServiceDefaults();

		// Add services to the container.
		builder.Services.AddRazorComponents()
			.AddInteractiveServerComponents();
		builder.Services.AddFluentUIComponents();

		builder.Services.AddHttpClient("SurveyService", client =>
		{
			// 从配置中获取 SurveyService 的端点 URL
			var url = builder.Configuration["Endpoint:SurveyService"]
					  ?? throw new InvalidOperationException("SurveyService endpoint is not configured.");
			client.BaseAddress = new Uri(url);
		});

		var app = builder.Build();

		app.MapDefaultEndpoints();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();

		app.UseAntiforgery();

		app.MapStaticAssets();
		app.MapRazorComponents<App>()
			.AddInteractiveServerRenderMode();

		app.Run();
	}
}
