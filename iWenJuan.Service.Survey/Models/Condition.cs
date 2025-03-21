using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iWenJuan.Shared.Enums;

namespace iWenJuan.Service.Survey.Models;

public class Condition
{
	[Key]
	public int ConditionId { get; set; }
	public Operator Operator { get; set; }
	public string Value { get; set; }
	public Effect Effect { get; set; }
	public int QuestionId { get; set; }
	[ForeignKey("QuestionId")]
	public Question Question { get; set; }
	public int? NextQuestionId { get; set; }
	[ForeignKey("NextQuestionId")]
	public Question NextQuestion { get; set; }

}
