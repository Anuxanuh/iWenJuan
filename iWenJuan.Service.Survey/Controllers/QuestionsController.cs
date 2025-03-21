using iWenJuan.Service.Survey.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace iWenJuan.Service.Survey.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionsController : ControllerBase
{
	private readonly ISurveyService surveyService;
	private readonly ILogger<QuestionsController> _logger;

	public QuestionsController(ISurveyService surveyService, ILogger<QuestionsController> logger)
	{
		this.surveyService = surveyService;
		_logger = logger;
	}

	// GET: api/<OptionsController>
	[HttpGet]
	public IEnumerable<string> Get()
	{
		return new string[] { "value1", "value2" };
	}

	// GET api/<OptionsController>/5
	[HttpGet("{id}")]
	public string Get(int id)
	{
		return "value";
	}

	// POST api/<OptionsController>
	[HttpPost]
	public void Post([FromBody] string value)
	{
	}

	// PUT api/<OptionsController>/5
	[HttpPut("{id}")]
	public void Put(int id, [FromBody] string value)
	{
	}

	// DELETE api/<OptionsController>/5
	[HttpDelete("{surveyId}/{id}")]
	public async Task<IActionResult> Delete(int surveyId, int id)
	{
		await surveyService.DeleteQuestionAsync(surveyId, id);

		return NoContent();
	}
}
