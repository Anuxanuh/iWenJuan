using iWenJuan.Service.Survey.Data;
using iWenJuan.Service.Survey.Extension;
using iWenJuan.Service.TemplateCommunity.Extension;
using JiebaNet.Segmenter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iWenJuan.Service.TemplateCommunity.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
	private readonly SurveyDbContext _context;
	private readonly ILogger<SearchController> _logger;

	public SearchController(SurveyDbContext context, ILogger<SearchController> logger)
	{
		_context = context;
		_logger = logger;
	}

	[HttpGet]
	public async Task<IActionResult> Get([FromQuery] string key)
	{
		// jieba 分词器
		var segmenter = new JiebaSegmenter();
		// 分词
		var keys = segmenter.CutForSearch(key);

		// 查询问卷并按匹配的关键词数量排序
		var searchResults = await _context.Surveys
			.AsNoTrackingWithIdentityResolution()
			.Where(s => keys.Any(k => s.Title.Contains(k) || (s.Description != null && s.Description.Contains(k))))
			.OrderByDescending(s => keys.Count(k => s.Title.Contains(k) || (s.Description != null && s.Description.Contains(k))))
			.Take(12)
			.Select(s => s.ToSearchResultDto())
			.ToListAsync();


		// SQL写法
		//var searchResults = await (
		//	from survey in _context.Surveys
		//	where keys.Any(k => survey.Title.Contains(k) ||
		//		(survey.Description != null && survey.Description.Contains(k)))
		//	orderby keys.Count(k => survey.Title.Contains(k) ||
		//		(survey.Description != null && survey.Description.Contains(k))) descending
		//	select survey
		//  ).AsNoTrackingWithIdentityResolution().Take(12).Select(s => s.ToSearchResultDto()).ToListAsync();


		// 记录日志
		_logger.LogInformation("搜索关键词: {key}, 匹配到问卷数量: {count}", string.Join("/", keys), searchResults.Count);

		// 返回结果
		if (searchResults.Count == 0)
		{
			return NotFound("没有找到相关问卷");
		}
		return Ok(searchResults);
	}
}
