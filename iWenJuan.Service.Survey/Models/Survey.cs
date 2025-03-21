using System.ComponentModel.DataAnnotations;

namespace iWenJuan.Service.Survey.Models;

public class Survey
{
	public int SurveyId { get; set; }
	[Required]
	public string Title { get; set; }
	public string? Description { get; set; }
	public bool IsPublished { get; set; } = false;
	[Required]
	public Guid CreatedBy { get; set; }
	[Required]
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public ICollection<Question> Questions { get; set; }
}
