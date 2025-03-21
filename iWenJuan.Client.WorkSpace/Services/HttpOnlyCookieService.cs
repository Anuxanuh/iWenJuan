using Microsoft.JSInterop;

namespace iWenJuan.Client.WorkSpace.Services;

public class HttpOnlyCookieService
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IJSRuntime JSRuntime;

	public HttpOnlyCookieService(IHttpContextAccessor httpContextAccessor, IJSRuntime jSRuntime)
	{
		_httpContextAccessor = httpContextAccessor;
		JSRuntime = jSRuntime;
	}

	/// <summary>
	/// 设置HttpOnly Cookie
	/// </summary>
	/// <param name="key">键</param>
	/// <param name="value">Cookie内容</param>
	/// <param name="expireTime">过期时间(Minutes), 不指定默认7天</param>
	public void SetCookie(string key, string value, int expireTime = 7 * 24 * 60)
	{
		CookieOptions option = new CookieOptions
		{
			HttpOnly = true,
			Secure = true, // 确保通过HTTPS发送Cookie
			/**
			 * Strict: 当SameSite设置为Strict时，Cookie只会在来自同一站点的请求中发送。在用户从外部站点导航到你的站点时，浏览器不会发送这些Cookie。这是最安全的选项，可以防止CSRF攻击，但也可能影响到某些用户体验。
			 * Lax: 当SameSite设置为Lax时，Cookie在来自第三方的导航请求中不会发送，但在用户主动导航到你的站点（如点击链接）时，浏览器仍然会发送这些Cookie。这种设置在提供一定程度的安全性的同时，不会严重影响用户体验。
			 * None: 当SameSite设置为None时，Cookie会在所有跨站请求中发送。这种设置提供最少的安全保护，因此在使用此选项时，必须确保同时设置Secure属性，将Cookie限制为通过HTTPS传输。
			 **/
			SameSite = SameSiteMode.Lax,
			Expires = DateTime.Now.AddMinutes(expireTime)
		};

		_httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
	}

	public string GetCookie(string key)
	{
		return _httpContextAccessor.HttpContext.Request.Cookies[key];
	}

	public void DeleteCookie(string key)
	{
		_httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
	}
}
