using System.ComponentModel.DataAnnotations;

namespace iWenJuan.Service.Survey.Models;

public class Answer
{
	public int AnswerId { get; set; }
	public int QuestionId { get; set; }
	public Question Question { get; set; }
	[Required]
	public Guid UserId { get; set; }
	[Required]
	public string AnswerText { get; set; }
	[Required]
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
