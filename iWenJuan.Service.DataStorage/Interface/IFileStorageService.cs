using iWenJuan.Service.DataStorage.Models;
using iWenJuan.Shared.Dtos;

namespace iWenJuan.Service.DataStorage.Interface;

/// <summary>
/// 文件存储服务接口
/// </summary>
public interface IFileStorageService
{
	/// <summary>
	/// 异步保存文件
	/// </summary>
	/// <param name="storedFile"></param>
	/// <returns></returns>
	Task<int> SaveFileAsync(StoredFile storedFile);

	/// <summary>
	/// 得到指定所有者的文件
	/// </summary>
	/// <param name="ownerId"></param>
	/// <returns></returns>
	Task<IEnumerable<StoredFileInfoDto>> GetFileInfosByOwnerIdAsync(Guid ownerId);

	/// <summary>
	/// 根据ID异步获取文件
	/// </summary>
	/// <param name="id">文件ID</param>
	/// <returns>存储的文件</returns>
	Task<StoredFile?> GetFileByIdAsync(int id);

	/// <summary>
	/// 异步删除文件
	/// </summary>
	/// <param name="id">文件ID</param>
	Task DeleteFileAsync(int id);
}
