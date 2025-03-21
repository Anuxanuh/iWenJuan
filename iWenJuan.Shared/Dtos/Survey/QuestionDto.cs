using iWenJuan.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWenJuan.Shared.Dtos;

public class QuestionDto
{
	public int QuestionId { get; set; }
	public int SurveyId { get; set; }
	public QuestionTypeEnum QuestionType { get; set; }
	public string QuestionText { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public ICollection<ConditionDto>? Conditions { get; set; }
	public ICollection<OptionDto>? Options { get; set; }
	public ICollection<AnswerDto>? Answers { get; set; }
}
