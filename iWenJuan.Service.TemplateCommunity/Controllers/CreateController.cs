using iWenJuan.Service.TemplateCommunity.Utils;
using iWenJuan.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iWenJuan.Service.TemplateCommunity.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CreateController : ControllerBase
{
	private readonly ILogger<CreateController> _logger;
	private readonly HttpClient _httpClient;

	public CreateController(ILogger<CreateController> logger, IHttpClientFactory httpClientFactory)
	{
		_logger = logger;
		_httpClient = httpClientFactory.CreateClient("SurveyService");
	}

	[HttpGet]
	public async Task<IActionResult> CreateSurveyFromTemplate([FromQuery] int surveyId, [FromQuery] Guid userId)
	{
		var response = await _httpClient.GetAsync($"api/Surveys/{surveyId}");

		if (!response.IsSuccessStatusCode)
			return NotFound("要复制的问卷不存在");

		var originalSurvey = await response.Content.ReadFromJsonAsync<SurveyDto>();

		if (originalSurvey == null)
			return NotFound("要复制的问卷不存在");

		var newSurvey = originalSurvey.Copy(userId);

		var createResponse = await _httpClient.PostAsJsonAsync("api/Surveys", newSurvey);

		if (!createResponse.IsSuccessStatusCode)
			return Problem("创建问卷出错");

		var uri = createResponse.Headers.Location;

		var createdSurvey = await createResponse.Content.ReadFromJsonAsync<SurveyDto>();

		return Created(uri, createdSurvey);
	}
}
