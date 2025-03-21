using iWenJuan.Service.Survey.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace iWenJuan.Service.Survey.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OptionsController : ControllerBase
{
	private readonly ISurveyService surveyService;
	private readonly ILogger<OptionsController> _logger;

	public OptionsController(ISurveyService surveyService, ILogger<OptionsController> logger)
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
    public void Post([FromBody]string value)
    {
    }

    // PUT api/<OptionsController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/<OptionsController>/5
    [HttpDelete("{questionId}/{id}")]
    public async Task<IActionResult> Delete(int questionId,int id)
    {
		await surveyService.DeleteOptionAsync(questionId, id);

		return NoContent();
	}
}
