using System.ComponentModel.DataAnnotations;

namespace iWenJuan.Service.Survey.Models;

public class Option
{
	public int OptionId { get; set; }
	public int QuestionId { get; set; }
	public Question Question { get; set; }
	[Required]
	public string OptionText { get; set; }
	[Required]
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
