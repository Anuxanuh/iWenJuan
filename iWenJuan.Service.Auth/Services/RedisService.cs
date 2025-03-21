using iWenJuan.Service.Auth.Interface;
using StackExchange.Redis;

namespace iWenJuan.Service.Auth.Services;

public class RedisService : IRedisService
{
	private readonly IConnectionMultiplexer _redis;

	public RedisService(IConnectionMultiplexer redis)
	{
		_redis = redis;
	}

	public async Task SetVerificationCodeAsync(string email, string code, TimeSpan expiry)
	{
		var db = _redis.GetDatabase();
		await db.StringSetAsync(email, code, expiry);
	}

	public async Task<string> GetVerificationCodeAsync(string email)
	{
		var db = _redis.GetDatabase();
		return await db.StringGetAsync(email);
	}

	public async Task<bool> VerifyVerificationCodeAsync(string email, string code)
	{
		var db = _redis.GetDatabase();
		var storedCode = await db.StringGetAsync(email);
		if (storedCode == code)
		{
			await db.KeyDeleteAsync(email);
			return true;
		}
		return false;
	}
}
