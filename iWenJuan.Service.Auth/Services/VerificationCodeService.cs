using iWenJuan.Service.Auth.Interface;
using iWenJuan.Shared.Dtos;
using StackExchange.Redis;
using System.Text.Json;

namespace iWenJuan.Service.Auth.Services;

public class VerificationCodeService : IVerificationCodeService
{
	private readonly IRedisService _redisService;
	private readonly IMessageQueueService _messageQueueService;

	public VerificationCodeService(IRedisService redisService, IMessageQueueService messageQueueService)
	{
		_redisService = redisService;
		_messageQueueService = messageQueueService;
	}

	private async Task<string> GetVerificationCode(string email)
	{
		var code = new Random().Next(100000, 999999).ToString();
		var expiryTime = TimeSpan.FromMinutes(5);

		await _redisService.SetVerificationCodeAsync(email, code, expiryTime);
		return code;
	}

	private void SendVerificationCode(string email, string code)
	{
		var message = JsonSerializer.Serialize(
			new EmailVerificationMessage
			{
				Email = email,
				Code = code
			});
		_messageQueueService.SendMessage(message);
	}

	public async Task<string> GetAndSendVerificationCode(string email)
	{
		var code = await GetVerificationCode(email);
		SendVerificationCode(email, code);
		return code;
	}

	public async Task<bool> VerifyVerificationCode(string email, string code)
	{
		return await _redisService.VerifyVerificationCodeAsync(email, code);
	}
}
