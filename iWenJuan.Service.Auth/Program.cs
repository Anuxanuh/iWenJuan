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
		// ���� WebApplicationBuilder ʵ��
		var builder = WebApplication.CreateBuilder(args);
		// ���Ĭ�Ϸ�������-.Net Aspire�ṩ��һЩĬ�Ϸ������ã�����ͨ������AddServiceDefaults�����������ЩĬ�Ϸ�������
		builder.AddServiceDefaults();

		#region ������������
		// ��ӿ���������
		builder.Services.AddControllers();
		// ��� OpenAPI ���������� API �ĵ�
		// �˽����������� OpenAPI ����Ϣ������� https://aka.ms/aspnet/openapi
		builder.Services.AddOpenApi();
		// �����־����
		builder.Services.AddLogging();
		#endregion ������������

		#region ���ݿ��������
		// ��� DbContext ��������ʹ�� PostgreSQL ���ݿ�
		builder.AddNpgsqlDbContext<AuthDbContext>("usrDb");
		#endregion ���ݿ��������

		#region �м������
		// ��� RabbitMQ �ͻ��˷���
		builder.AddRabbitMQClient("mq");
		// ��� Redis �ͻ��˷���
		builder.AddRedisClient("redis");
		// ��� ��Ϣ���� ����
		builder.Services.AddScoped<IMessageQueueService, RabbitMQService>();
		// ��� RedisService ����
		builder.Services.AddScoped<IRedisService, RedisService>();
		// ��� VerificationCodeService ����
		builder.Services.AddScoped<IVerificationCodeService, VerificationCodeService>();
		#endregion �м������

		#region ��֤��������
		// ��� JwtBearer ��֤����
		builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				// ����������֤����
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true, // ��֤������
					ValidateAudience = true, // ��֤����
					ValidateLifetime = true, // ��֤������Ч��
					ValidateIssuerSigningKey = true, // ��֤������ǩ����Կ
					ValidIssuer = builder.Configuration["Jwt:Issuer"], // ������Ч�ķ�����
					ValidAudience = builder.Configuration["Jwt:Audience"], // ������Ч������
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
						builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("AuthService endpoint is not configured."))) // ���÷�����ǩ����Կ
				};
			});
		// ��� PasswordHasher ����
		builder.Services.AddSingleton<IPasswordHasher, PasswordHasherWithArgon2>();
		// ��� JwtTokenGenerator ����
		builder.Services.AddSingleton<IJwtTokenHelper<User>, JwtTokenHelper>();
		#endregion ��֤��������

		// ���� WebApplication ʵ��
		var app = builder.Build();

		#region ���ݿ��Զ�Ǩ��
		using (var scope = app.Services.CreateScope())
		{
			var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
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

		// ������֤�м��
		app.UseAuthentication();
		// ������Ȩ�м��
		app.UseAuthorization();

		// ӳ��������˵�
		app.MapControllers();
		// ����Ӧ�ó���
		app.Run();
	}
}
