namespace iWenJuan.Shared.Dtos;

public class StoredFileInfoDto
{
	public int Id { get; set; }
	public string FileName { get; set; }
	public Guid Owner { get; set; }
	public string ContentType { get; set; }
	public DateTime CreatedAt { get; set; }
}
