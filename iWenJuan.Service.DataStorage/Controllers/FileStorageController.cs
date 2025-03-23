using iWenJuan.Service.DataStorage.Data;
using iWenJuan.Service.DataStorage.Interface;
using iWenJuan.Service.DataStorage.Models;
using iWenJuan.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Mime;

namespace iWenJuan.Service.DataStorage.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileStorageController : ControllerBase
{
	private readonly IFileStorageService _fileStorageService;
	private readonly IExportService _exportService;

	/// <summary>
	/// 构造函数，注入所需的服务
	/// </summary>
	/// <param name="fileStorageService">文件存储服务</param>
	/// <param name="exportService">导出服务</param>
	public FileStorageController(IFileStorageService fileStorageService, IExportService exportService)
	{
		_fileStorageService = fileStorageService;
		_exportService = exportService;
	}

	/// <summary>
	/// 保存指定问卷的答题数据到数据库
	/// </summary>
	/// <param name="Owner">文件所有者</param>
	/// <param name="SurveyId">调查ID</param>
	/// <returns>返回文件ID</returns>
	[HttpPost("exportSurveyResult")]
	public async Task<IActionResult> ExportSurveyResultToDatabase([FromBody] ExportSurveyResultDto exportSurveyResult)
	{
		// 导出调查结果为CSV数据
		var csvData = await _exportService.ExportSurveyResultAsync(exportSurveyResult.SurveyId);
		// 保存文件并获取存储的文件ID
		var storedFileId = await _fileStorageService.SaveFileAsync(new StoredFile
		{
			Owner = exportSurveyResult.Owner,
			FileName = $"surveyResult_{exportSurveyResult.SurveyId}_{DateTime.Now:yyyyMMddHHmmss}.csv",
			ContentType = "text/csv",
			Data = csvData
		});
		// 返回文件ID
		return Ok(new StoredFileInfoDto
		{
			Id = storedFileId
		});
	}

	/// <summary>
	/// 一个妥协的方法
	/// 为oldFileId的主人保存一个新文件
	/// 实际上，这个方法应该接受一个文件的所有者，而不是使用旧文件ID
	/// </summary>
	/// <param name="oldFileId"></param>
	/// <param name="newFile"></param>
	/// <returns></returns>
	[HttpPost("save/{oldFileId}")]
	public async Task<IActionResult> SaveFileToDatabase(int oldFileId, [FromForm] IFormFile newFile)
	{
		// 根据文件ID获取文件
		var oldStoredFile = await _fileStorageService.GetFileByIdAsync(oldFileId);
		// 如果文件不存在，返回404
		if (oldStoredFile == null)
			return NotFound();

		// 保存文件并获取存储的文件ID
		byte[] fileData;
		using (var memoryStream = new MemoryStream())
		{
			await newFile.CopyToAsync(memoryStream);
			fileData = memoryStream.ToArray();
		}
		// 保存文件并获取存储的文件ID
		var newStoredFileId = await _fileStorageService.SaveFileAsync(new StoredFile
		{
			Owner = oldStoredFile.Owner,
			FileName = $"newFile_{DateTime.Now:yyyyMMddHHmmss}.csv",
			ContentType = "text/csv",
			Data = fileData
		});
		// 返回文件ID
		return Ok(new StoredFileInfoDto { Id = newStoredFileId });
	}

	/// <summary>
	/// 获取指定所有者的文件列表
	/// </summary>
	/// <param name="owner">文件所有者</param>
	/// <returns>返回文件列表</returns>
	[HttpGet("file/{owner}/list")]
	public async Task<IActionResult> GetFileList(Guid owner)
	{
		// 查询数据库中指定所有者的文件列表
		var storedFiles = await _fileStorageService.GetFileInfosByOwnerIdAsync(owner);
		// 返回文件列表
		return Ok(storedFiles);
	}

	/// <summary>
	/// 获取指定ID的文件信息
	/// </summary>
	/// <param name="fileId">文件ID</param>
	/// <returns>返回文件信息</returns>
	[HttpGet("file/{fileId}/info")]
	public async Task<IActionResult> GetFileInformation(int fileId)
	{
		var storedFile = await _fileStorageService.GetFileByIdAsync(fileId);
		if (storedFile == null)
		{
			return NotFound();
		}
		return Ok(new StoredFileInfoDto
		{
			Id = storedFile.Id,
			FileName = storedFile.FileName,
			Owner = storedFile.Owner,
			ContentType = storedFile.ContentType,
			CreatedAt = storedFile.CreatedAt
		});
	}

	/// <summary>
	/// 获取指定ID的文件
	/// </summary>
	/// <param name="fileId">文件ID</param>
	/// <returns>返回文件数据</returns>
	[HttpGet("file/{fileId}/data")]
	public async Task<IActionResult> GetFile(int fileId)
	{
		// 根据文件ID获取文件
		var storedFile = await _fileStorageService.GetFileByIdAsync(fileId);
		if (storedFile == null)
		{
			// 如果文件不存在，返回404
			return NotFound();
		}

		// 返回文件数据
		return File(storedFile.Data, storedFile.ContentType, storedFile.FileName);
	}

	/// <summary>
	/// 删除指定ID的文件
	/// </summary>
	/// <param name="fileId"></param>
	/// <returns></returns>
	[HttpDelete("file/{fileId}")]
	public async Task<IActionResult> DeleteFile(int fileId)
	{
		// 根据文件ID获取文件
		var storedFile = await _fileStorageService.GetFileByIdAsync(fileId);
		if (storedFile == null)
		{
			// 如果文件不存在，返回404
			return NotFound();
		}
		_ = _fileStorageService.DeleteFileAsync(fileId);
		return Ok();
	}
}
