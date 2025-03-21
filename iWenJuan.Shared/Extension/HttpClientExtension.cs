using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace iWenJuan.Shared.Extension;

public static class HttpClientExtension
{
	public static async Task<HttpResponseMessage> GetWithParmsAsync(this HttpClient client, string url, object parms)
	{
		// 创建 UriBuilder 对象并设置初始 URL
		var uriBuilder = new UriBuilder(url);

		// 解析 URL 中的查询字符串参数
		var query = string.IsNullOrEmpty(uriBuilder.Query)
			? HttpUtility.ParseQueryString(uriBuilder.Query)
			: [];

		// 遍历参数对象的所有属性，将属性名和属性值添加到查询参数中
		foreach (var prop in parms.GetType().GetProperties())
		{
			query[prop.Name] = prop.GetValue(parms)?.ToString();
		}

		// 将生成的查询参数重新设置回 UriBuilder 对象
		uriBuilder.Query = query.ToString();

		// 构建完整的 URL
		var fullUrl = uriBuilder.ToString();

		// 发送 GET 请求并返回响应消息
		return await client.GetAsync(fullUrl);
	}
}
