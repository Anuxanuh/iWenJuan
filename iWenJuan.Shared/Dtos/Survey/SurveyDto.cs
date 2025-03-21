using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWenJuan.Shared.Dtos;

public class SurveyDto
{
	public int SurveyId { get; set; }
	public string Title { get; set; }
	public string? Description { get; set; }
	public bool IsPublished { get; set; } = false;
	public Guid CreatedBy { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public ICollection<QuestionDto>? Questions { get; set; }
}
