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

		#region �����֤
		builder.Services.AddAuthorization();
		builder.Services.AddCascadingAuthenticationState();
		builder.Services.AddHttpContextAccessor();

		builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
		builder.Services.AddScoped<AuthenticationService>();
		builder.Services.AddScoped<HttpOnlyCookieService>();
		#endregion �����֤

		#region ����΢����HttpClient
		builder.Services.AddHttpClient("AuthService", client =>
		{
			// �������л�ȡ AuthService �Ķ˵� URL
			var url = builder.Configuration["Endpoint:AuthService"]
					  ?? throw new InvalidOperationException("AuthService endpoint is not configured.");
			client.BaseAddress = new Uri(url);
		});
		builder.Services.AddHttpClient("SurveyService", client =>
		{
			// �������л�ȡ SurveyService �Ķ˵� URL
			var url = builder.Configuration["Endpoint:SurveyService"]
					  ?? throw new InvalidOperationException("SurveyService endpoint is not configured.");
			client.BaseAddress = new Uri(url);
		});
		builder.Services.AddHttpClient("DataStoreService", client =>
		{
			// �������л�ȡ DataStoreService �Ķ˵� URL
			var url = builder.Configuration["Endpoint:DataStoreService"]
					  ?? throw new InvalidOperationException("DataStoreService endpoint is not configured.");
			client.BaseAddress = new Uri(url);
		});
		builder.Services.AddHttpClient("DataProcessingService", client =>
		{
			// �������л�ȡ DataProcessingService �Ķ˵� URL
			var url = builder.Configuration["Endpoint:DataProcessingService"]
					  ?? throw new InvalidOperationException("DataProcessingService endpoint is not configured.");
			client.BaseAddress = new Uri(url);
		});
		builder.Services.AddHttpClient("TemplateCommunityService", client =>
		{
			// �������л�ȡ TemplateCommunityService �Ķ˵� URL
			var url = builder.Configuration["Endpoint:TemplateCommunityService"]
					  ?? throw new InvalidOperationException("TemplateCommunityService endpoint is not configured.");
			client.BaseAddress = new Uri(url);
		});
		//builder.Services.AddHttpClient("AnswerClient", client =>
		//{
		//	// �������л�ȡ AnswerClient �Ķ˵� URL
		//	var url = builder.Configuration["Endpoint:AnswerClient"]
		//			  ?? throw new InvalidOperationException("AnswerClient endpoint is not configured.");
		//	client.BaseAddress = new Uri(url);
		//});
		#endregion ����΢����HttpClient

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
