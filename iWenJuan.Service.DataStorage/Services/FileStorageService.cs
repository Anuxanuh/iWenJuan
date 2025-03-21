using iWenJuan.Service.DataStorage.Data;
using iWenJuan.Service.DataStorage.Interface;
using iWenJuan.Service.DataStorage.Models;

namespace iWenJuan.Service.DataStorage.Services;

public class FileStorageService : IFileStorageService
{
	private readonly StorageDbContext _context;

	/// <summary>
	/// 初始化文件存储服务的新实例
	/// </summary>
	/// <param name="context">数据库上下文</param>
	public FileStorageService(StorageDbContext context)
	{
		_context = context;
	}

	/// <summary>
	/// 异步保存文件
	/// </summary>
	/// <param name="owner">文件所有者的唯一标识符</param>
	/// <param name="fileName">文件名</param>
	/// <param name="contentType">文件的MIME类型</param>
	/// <param name="data">文件的字节数据</param>
	/// <returns>保存的文件的唯一标识符</returns>
	public async Task<int> SaveFileAsync(Guid owner, string fileName, string contentType, byte[] data)
	{
		// 创建一个新的存储文件实例
		var storedFile = new StoredFile
		{
			Owner = owner,
			FileName = fileName,
			ContentType = contentType,
			Data = data
		};

		// 将文件添加到数据库上下文
		_context.StoredFiles.Add(storedFile);
		// 异步保存更改到数据库
		await _context.SaveChangesAsync();

		// 返回保存的文件的唯一标识符
		return storedFile.Id;
	}

	/// <summary>
	/// 根据文件ID异步获取文件
	/// </summary>
	/// <param name="id">文件的唯一标识符</param>
	/// <returns>存储的文件，如果未找到则返回null</returns>
	public async Task<StoredFile?> GetFileByIdAsync(int id) => await _context.StoredFiles.FindAsync(id);
}
