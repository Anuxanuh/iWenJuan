using iWenJuan.Service.DataStorage.Interface;

namespace iWenJuan.Service.DataStorage.Services;

public class ExportService : IExportService
{
	private readonly HttpClient _httpClient;

	/// <summary>
	/// 构造函数，初始化HttpClient
	/// </summary>
	/// <param name="httpClientFactory">HttpClient工厂</param>
	public ExportService(IHttpClientFactory httpClientFactory)
	{
		_httpClient = httpClientFactory.CreateClient("SurveyService");
	}

	/// <summary>
	/// 导出问卷结果为字节数组
	/// </summary>
	/// <param name="surveyId">问卷ID</param>
	/// <returns>包含问卷结果的字节数组</returns>
	public async Task<byte[]> ExportSurveyResultAsync(int surveyId)
	{
		// 发送GET请求获取问卷结果数据
		var response = await _httpClient.GetByteArrayAsync($"api/export/csv/{surveyId}");
		return response ?? throw new HttpRequestException("Failed to retrieve survey result.");
	}
}
