namespace iWenJuan.Service.Survey.Interface;

/// <summary>
/// 导出服务接口
/// </summary>
public interface IExportService
{
	/// <summary>
	/// 异步导出为CSV
	/// </summary>
	/// <param name="surveyId">问卷ID</param>
	/// <returns>CSV文件的字节数组</returns>
	Task<byte[]> ExportToCsvAsync(int surveyId);

	/// <summary>
	/// 异步导出为XLS
	/// </summary>
	/// <param name="surveyId">问卷ID</param>
	/// <returns>XLS文件的字节数组</returns>
	Task<byte[]> ExportToXlsAsync(int surveyId);
}
