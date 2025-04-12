using Microsoft.FluentUI.AspNetCore.Components;
using iWenJuan.Client.WorkSpace.Components;
using Microsoft.AspNetCore.Components.Authorization;
using iWenJuan.Client.WorkSpace.Services;
using MudBlazor.Services;

namespace iWenJuan.Client.WorkSpace;

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
		builder.Services.AddMudServices();
		builder.Services.AddLogging();

		#region 身份认证
		builder.Services.AddAuthorization();
		builder.Services.AddCascadingAuthenticationState();
		builder.Services.AddHttpContextAccessor();

		builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
		builder.Services.AddScoped<AuthenticationService>();
		builder.Services.AddScoped<HttpOnlyCookieService>();
		#endregion 身份认证

		#region 其他微服务HttpClient
		builder.Services.AddHttpClient("AuthService", client =>
		{
			// 从配置中获取 AuthService 的端点 URL
			var url = builder.Configuration["Endpoint:AuthService"]
					  ?? throw new InvalidOperationException("AuthService endpoint is not configured.");
			client.BaseAddress = new Uri(url);
		});
		builder.Services.AddHttpClient("SurveyService", client =>
		{
			// 从配置中获取 SurveyService 的端点 URL
			var url = builder.Configuration["Endpoint:SurveyService"]
					  ?? throw new InvalidOperationException("SurveyService endpoint is not configured.");
			client.BaseAddress = new Uri(url);
		});
		builder.Services.AddHttpClient("DataStoreService", client =>
		{
			// 从配置中获取 DataStoreService 的端点 URL
			var url = builder.Configuration["Endpoint:DataStoreService"]
					  ?? throw new InvalidOperationException("DataStoreService endpoint is not configured.");
			client.BaseAddress = new Uri(url);
		});
		builder.Services.AddHttpClient("DataProcessingService", client =>
		{
			// 从配置中获取 DataProcessingService 的端点 URL
			var url = builder.Configuration["Endpoint:DataProcessingService"]
					  ?? throw new InvalidOperationException("DataProcessingService endpoint is not configured.");
			client.BaseAddress = new Uri(url);
		});
		builder.Services.AddHttpClient("TemplateCommunityService", client =>
		{
			// 从配置中获取 TemplateCommunityService 的端点 URL
			var url = builder.Configuration["Endpoint:TemplateCommunityService"]
					  ?? throw new InvalidOperationException("TemplateCommunityService endpoint is not configured.");
			client.BaseAddress = new Uri(url);
		});
		//builder.Services.AddHttpClient("AnswerClient", client =>
		//{
		//	// 从配置中获取 AnswerClient 的端点 URL
		//	var url = builder.Configuration["Endpoint:AnswerClient"]
		//			  ?? throw new InvalidOperationException("AnswerClient endpoint is not configured.");
		//	client.BaseAddress = new Uri(url);
		//});
		#endregion 其他微服务HttpClient

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

		//app.UseAuthentication();
		//app.UseAuthorization();

		app.UseAntiforgery();

		app.MapStaticAssets();
		app.MapRazorComponents<App>()
			.AddInteractiveServerRenderMode();

		app.Run();
	}
}
