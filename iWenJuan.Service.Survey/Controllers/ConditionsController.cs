using iWenJuan.Service.Survey.Interface;
using iWenJuan.Service.Survey.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace iWenJuan.Service.Survey.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConditionsController : ControllerBase
{
	private readonly ISurveyService surveyService;
	private readonly ILogger<ConditionsController> _logger;

	public ConditionsController(ISurveyService surveyService, ILogger<ConditionsController> logger)
	{
		this.surveyService = surveyService;
		_logger = logger;
	}


	// GET: api/<ConditonsController>
	[HttpGet]
	public IEnumerable<string> Get()
	{
		return new string[] { "value1", "value2" };
	}

	// GET api/<ConditonsController>/5
	[HttpGet("{id}")]
	public string Get(int id)
	{
		return "value";
	}

	// POST api/<ConditonsController>
	[HttpPost]
	public void Post([FromBody] string value)
	{
	}

	// PUT api/<ConditonsController>/5
	[HttpPut("{id}")]
	public void Put(int id, [FromBody] string value)
	{
	}

	// DELETE api/<ConditonsController>/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		await surveyService.DeleteConditionAsync(id);

		return NoContent();
	}
}
