using iWenJuan.Service.Survey.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace iWenJuan.Service.Survey.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExportController : ControllerBase
{
	private readonly IExportService _exportService;

	/// <summary>
	/// 初始化 ExportController 类的新实例。
	/// </summary>
	/// <param name="exportService">导出服务的实例。</param>
	public ExportController(IExportService exportService)
	{
		_exportService = exportService;
	}

	/// <summary>
	/// 导出问卷结果为 CSV 文件。
	/// </summary>
	/// <param name="surveyId">要导出的调查的ID。</param>
	/// <returns>包含 CSV 文件的 IActionResult。</returns>
	[HttpGet("csv/{surveyId}")]
	public async Task<IActionResult> ExportToCsv(int surveyId)
	{
		try
		{
			// 调用导出服务获取 CSV 数据
			var csvData = await _exportService.ExportToCsvAsync(surveyId);
			// 返回包含 CSV 文件的响应
			return File(csvData, "text/csv",
				$"SurveyResult_{User.FindFirst(ClaimTypes.Name)?.Value}_{surveyId}.csv");
		}
		catch (Exception ex)
		{
			// 如果发生异常，返回 404 错误和异常消息
			return NotFound(ex.Message);
		}
	}

	/// <summary>
	/// 导出问卷结果为 XLS 文件。
	/// </summary>
	/// <param name="surveyId">要导出的调查的ID。</param>
	/// <returns>包含 XLS 文件的 IActionResult。</returns>
	[HttpGet("xls/{surveyId}")]
	public async Task<IActionResult> ExportToXls(int surveyId)
	{
		try
		{
			// 调用导出服务获取 XLS 数据
			var xlsData = await _exportService.ExportToXlsAsync(surveyId);
			// 返回包含 XLS 文件的响应
			return File(xlsData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
				$"SurveyResult_{User.FindFirst(ClaimTypes.Name)?.Value}_{surveyId}.xlsx");
		}
		catch (Exception ex)
		{
			// 如果发生异常，返回 404 错误和异常消息
			return NotFound(ex.Message);
		}
	}
}
