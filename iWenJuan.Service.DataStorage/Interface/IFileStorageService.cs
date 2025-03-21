using iWenJuan.Service.DataStorage.Models;

namespace iWenJuan.Service.DataStorage.Interface;

/// <summary>
/// 文件存储服务接口
/// </summary>
public interface IFileStorageService
{
	/// <summary>
	/// 异步保存文件
	/// </summary>
	/// <param name="owner">文件所有者</param>
	/// <param name="fileName">文件名</param>
	/// <param name="contentType">文件类型</param>
	/// <param name="data">文件数据</param>
	/// <returns>文件ID</returns>
	Task<int> SaveFileAsync(Guid owner, string fileName, string contentType, byte[] data);

	/// <summary>
	/// 根据ID异步获取文件
	/// </summary>
	/// <param name="id">文件ID</param>
	/// <returns>存储的文件</returns>
	Task<StoredFile?> GetFileByIdAsync(int id);
}
