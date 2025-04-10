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

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] string title)
	{
		// 创建问卷
		var response = await _httpClient.PostAsJsonAsync("api/Survey", new { Title = title });
		if (response.IsSuccessStatusCode)
		{
			return Ok("问卷创建成功");
		}
		else
		{
			return BadRequest("问卷创建失败");
		}
	}
}
