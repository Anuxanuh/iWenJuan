using iWenJuan.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWenJuan.Shared.Dtos;

public class ConditionDto
{
	public int ConditionId { get; set; }
	public Operator Operator { get; set; }
	public string Value { get; set; }
	public Effect Effect { get; set; }
	public int QuestionId { get; set; }
	public int? NextQuestionId { get; set; }
}
