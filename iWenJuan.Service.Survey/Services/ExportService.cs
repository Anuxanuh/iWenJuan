using CsvHelper;
using CsvHelper.Configuration;
using iWenJuan.Service.Survey.Data;
using iWenJuan.Service.Survey.Interface;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Text;

namespace iWenJuan.Service.Survey.Services;

public class ExportService : IExportService
{
	private readonly SurveyDbContext _context;

	public ExportService(SurveyDbContext context)
	{
		_context = context;
	}

	/// <summary>
	/// 导出问卷数据到CSV文件。
	/// </summary>
	/// <param name="surveyId">要导出的问卷ID。</param>
	/// <returns>表示CSV文件的字节数组。</returns>
	/// <exception cref="KeyNotFoundException">当未找到问卷时抛出。</exception>
	public async Task<byte[]> ExportToCsvAsync(int surveyId)
	{
		// 从数据库中检索包含问题和答案的问卷
		var survey = await _context.Surveys.Include(s => s.Questions)
										   .ThenInclude(q => q.Answers)
										   .FirstOrDefaultAsync(s => s.SurveyId == surveyId)
										   ?? throw new KeyNotFoundException("未找到对应问卷");

		using var memoryStream = new MemoryStream();
		using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
		using var csv = new CsvWriter(writer, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture));

		// 获取按QuestionId排序的问题列表
		var questions = survey.Questions.OrderBy(q => q.QuestionId).ToList();
		// 按UserId分组答案
		var answersGroupedByUser = questions.SelectMany(q => q.Answers)
											.GroupBy(a => a.UserId)
											.ToList();

		// 写入表头行
		csv.WriteField("UserId");
		foreach (var question in questions)
		{
			csv.WriteField(question.QuestionText);
		}
		csv.NextRecord();

		// 写入每个用户的答案
		foreach (var userAnswers in answersGroupedByUser)
		{
			csv.WriteField(userAnswers.Key); // 写入UserId
			foreach (var question in questions)
			{
				var answer = userAnswers.FirstOrDefault(a => a.QuestionId == question.QuestionId);
				csv.WriteField(answer?.AnswerText ?? ""); // 写入答案文本或为空
			}
			csv.NextRecord();
		}

		await writer.FlushAsync();
		return memoryStream.ToArray();
	}

	/// <summary>
	/// 导出问卷数据到Excel文件。
	/// </summary>
	/// <param name="surveyId">要导出的问卷ID。</param>
	/// <returns>表示Excel文件的字节数组。</returns>
	/// <exception cref="KeyNotFoundException">当未找到问卷时抛出。</exception>
	public async Task<byte[]> ExportToXlsAsync(int surveyId)
	{
		// 从数据库中检索包含问题和答案的问卷
		var survey = await _context.Surveys.Include(s => s.Questions)
										   .ThenInclude(q => q.Answers)
										   .FirstOrDefaultAsync(s => s.SurveyId == surveyId)
										   ?? throw new KeyNotFoundException("未找到对应问卷");

		using var package = new ExcelPackage();
		// 创建一个以问卷标题命名的工作表
		var worksheet = package.Workbook.Worksheets.Add(survey.Title);
		// 获取按QuestionId排序的问题列表
		var questions = survey.Questions.OrderBy(q => q.QuestionId).ToList();
		// 按UserId分组答案
		var answersGroupedByUser = questions.SelectMany(q => q.Answers)
											.GroupBy(a => a.UserId)
											.ToList();
		// 初始化行和列
		var row = 1;
		var col = 1;

		// 写入表头行
		worksheet.Cells[row, col++].Value = "UserId";
		foreach (var question in questions)
		{
			worksheet.Cells[row, col++].Value = question.QuestionText;
		}

		// 写入每个用户的答案
		foreach (var userAnswers in answersGroupedByUser)
		{
			row++;
			col = 1;
			worksheet.Cells[row, col++].Value = userAnswers.Key; // 写入UserId
			foreach (var question in questions)
			{
				var answer = userAnswers.FirstOrDefault(a => a.QuestionId == question.QuestionId);
				worksheet.Cells[row, col++].Value = answer?.AnswerText ?? ""; // 写入答案文本或为空
			}
		}

		// 返回Excel文件
		return package.GetAsByteArray();
	}
}
