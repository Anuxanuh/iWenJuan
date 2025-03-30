using iWenJuan.Service.DataProcessing.Interface;
using iWenJuan.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace iWenJuan.Service.DataProcessing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CsvProcessingController : ControllerBase
{
	private readonly ICsvProcessingService _csvProcessingService;
	private readonly HttpClient _httpClient;
	private readonly ILogger<CsvProcessingController> _logger;

	public CsvProcessingController(ICsvProcessingService csvProcessingService, IHttpClientFactory httpClientFactory, ILogger<CsvProcessingController> logger)
	{
		_csvProcessingService = csvProcessingService;
		_httpClient = httpClientFactory.CreateClient("DataStoreService");
		_logger = logger;
	}

	[HttpPost("process/{fileId}")]
	public async Task<IActionResult> ProcessCsv(int fileId, [FromBody] List<CsvOperation> operations)
	{
		_logger.LogError($"{JsonSerializer.Serialize(operations)}");

		var file = await _httpClient.GetStreamAsync($"api/FileStorage/file/{fileId}/data");

		if (file == null || operations == null)
		{
			return BadRequest("文件/操作队列为null");
		}

		var processedCsv = await _csvProcessingService.ProcessCsvAsync(file, operations);

		// 将 byte[] 转换为 ByteArrayContent
		var fileContent = new ByteArrayContent(processedCsv);
		fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

		// 创建 MultipartFormDataContent
		var formData = new MultipartFormDataContent
		{
			{ fileContent, "newFile", "processedFile.csv" }
		};

		var response = await _httpClient.PostAsync($"api/FileStorage/save/{fileId}", formData);

		response.EnsureSuccessStatusCode();

		var newFileInfo = await response.Content.ReadFromJsonAsync<StoredFileInfoDto>();

		return Ok(newFileInfo);
	}
}
