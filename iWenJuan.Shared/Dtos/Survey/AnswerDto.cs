using System.ComponentModel.DataAnnotations;

namespace iWenJuan.Shared.Dtos;

public class AnswerDto
{
	public int AnswerId { get; set; }
	public int QuestionId { get; set; }
	public Guid? UserId { get; set; }
	public string AnswerText { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
