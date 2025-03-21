using System.ComponentModel.DataAnnotations;

namespace iWenJuan.Service.DataStorage.Models;

public class StoredFile
{
	public int Id { get; set; }
	[Required]
	public string FileName { get; set; }
	[Required]
	public Guid Owner { get; set; }
	[Required]
	public string ContentType { get; set; }
	[Required]
	public byte[] Data { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
