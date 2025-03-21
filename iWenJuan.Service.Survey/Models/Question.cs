using iWenJuan.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace iWenJuan.Service.Survey.Models;

public class Question
{
	public int QuestionId { get; set; }
	public int SurveyId { get; set; }
	public Survey Survey { get; set; }
	[Required]
	public QuestionTypeEnum QuestionType { get; set; }
	[Required]
	public string QuestionText { get; set; }
	[Required]
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public ICollection<Condition>? Conditions { get; set; }

	public ICollection<Option>? Options { get; set; }

	public ICollection<Answer>? Answers { get; set; }
}
