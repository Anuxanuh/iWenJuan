namespace iWenJuan.Service.DataStorage.Interface;

/// <summary>
/// 导出问卷结果的接口。
/// </summary>
public interface IExportService
{
	/// <summary>
	/// 将问卷结果导出为字节数组。
	/// </summary>
	/// <param name="surveyId">要导出的调查的ID。</param>
	/// <returns>表示异步操作的任务。任务结果包含导出的问卷结果的字节数组。</returns>
	Task<byte[]> ExportSurveyResultAsync(int surveyId);
}
