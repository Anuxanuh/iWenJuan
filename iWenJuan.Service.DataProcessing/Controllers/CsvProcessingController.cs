using iWenJuan.Service.DataProcessing.Interface;
using iWenJuan.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace iWenJuan.Service.DataProcessing.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CsvProcessingController : ControllerBase
{
	private readonly ICsvProcessingService _csvProcessingService;
	private readonly HttpClient _httpClient;

	public CsvProcessingController(ICsvProcessingService csvProcessingService, IHttpClientFactory httpClientFactory)
	{
		_csvProcessingService = csvProcessingService;
		_httpClient = httpClientFactory.CreateClient("DataStoreService");
	}

	[HttpPost("process/{fileId}")]
	public async Task<IActionResult> ProcessCsv(int fileId, [FromBody] List<CsvOperation> operations)
	{
		var file = await _httpClient.GetStreamAsync($"api/file/{fileId}/data");

		if (file == null || operations == null)
		{
			return BadRequest("文件/操作队列为null");
		}

		var processedCsv = await _csvProcessingService.ProcessCsvAsync(file, operations);

		var httpContent = new ByteArrayContent(processedCsv);

		var response = await _httpClient.PostAsync($"api/save/{fileId}", httpContent);

		response.EnsureSuccessStatusCode();

		var newFileInfo = await response.Content.ReadFromJsonAsync<StoredFileInfoDto>();

		return Ok(newFileInfo);
	}
}
