using iWenJuan.Service.Auth.Data;
using iWenJuan.Service.Auth.Interface;
using iWenJuan.Service.Auth.Models;
using iWenJuan.Service.Auth.Services;
using iWenJuan.Service.Auth.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace iWenJuan.Service.Auth;

public class Program
{
	public static void Main(string[] args)
	{
		// 创建 WebApplicationBuilder 实例
		var builder = WebApplication.CreateBuilder(args);
		// 添加默认服务配置-.Net Aspire提供了一些默认服务配置，可以通过调用AddServiceDefaults方法来添加这些默认服务配置
		builder.AddServiceDefaults();

		#region 基础服务配置
		// 添加控制器服务
		builder.Services.AddControllers();
		// 添加 OpenAPI 服务以生成 API 文档
		// 了解更多关于配置 OpenAPI 的信息，请访问 https://aka.ms/aspnet/openapi
		builder.Services.AddOpenApi();
		// 添加日志服务
		builder.Services.AddLogging();
		#endregion 基础服务配置

		#region 数据库服务配置
		// 添加 DbContext 服务并配置使用 PostgreSQL 数据库
		builder.AddNpgsqlDbContext<AuthDbContext>("usrDb");
		#endregion 数据库服务配置

		#region 中间件配置
		// 添加 RabbitMQ 客户端服务
		builder.AddRabbitMQClient("mq");
		// 添加 Redis 客户端服务
		builder.AddRedisClient("redis");
		// 添加 消息队列 服务
		builder.Services.AddScoped<IMessageQueueService, RabbitMQService>();
		// 添加 RedisService 服务
		builder.Services.AddScoped<IRedisService, RedisService>();
		// 添加 VerificationCodeService 服务
		builder.Services.AddScoped<IVerificationCodeService, VerificationCodeService>();
		#endregion 中间件配置

		#region 认证服务配置
		// 添加 JwtBearer 认证服务
		builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				// 配置令牌验证参数
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true, // 验证发行者
					ValidateAudience = true, // 验证受众
					ValidateLifetime = true, // 验证令牌有效期
					ValidateIssuerSigningKey = true, // 验证发行者签名密钥
					ValidIssuer = builder.Configuration["Jwt:Issuer"], // 设置有效的发行者
					ValidAudience = builder.Configuration["Jwt:Audience"], // 设置有效的受众
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
						builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("AuthService endpoint is not configured."))) // 设置发行者签名密钥
				};
			});
		// 添加 PasswordHasher 服务
		builder.Services.AddSingleton<IPasswordHasher, PasswordHasherWithArgon2>();
		// 添加 JwtTokenGenerator 服务
		builder.Services.AddSingleton<IJwtTokenHelper<User>, JwtTokenHelper>();
		#endregion 认证服务配置

		// 构建 WebApplication 实例
		var app = builder.Build();

		#region 数据库自动迁移
		using (var scope = app.Services.CreateScope())
		{
			var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
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

		// 启用认证中间件
		app.UseAuthentication();
		// 启用授权中间件
		app.UseAuthorization();

		// 映射控制器端点
		app.MapControllers();
		// 运行应用程序
		app.Run();
	}
}
