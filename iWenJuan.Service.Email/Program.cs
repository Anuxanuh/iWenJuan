using iWenJuan.Service.Email.Interface;
using iWenJuan.Service.Email.Services;

namespace iWenJuan.Service.Email;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// 添加服务默认配置
		builder.AddServiceDefaults();

		// 添加 RabbitMQ 客户端服务
		builder.AddRabbitMQClient("mq");

		// 添加 EmailService 服务
		builder.Services.AddSingleton<IEmailService, EmailService>();

		// 添加 消息队列 服务
		builder.Services.AddHostedService<RabbitMQConsumerService>();

		builder.Services.AddLogging();

		var app = builder.Build();

		// 映射默认端点
		app.MapDefaultEndpoints();

		// 使用 HTTPS 重定向
		app.UseHttpsRedirection();

		// 映射根路径的 GET 请求，返回服务运行状态
		app.MapGet("/", () => "Email Service is running");

		// 运行应用程序
		app.Run();
	}
}
