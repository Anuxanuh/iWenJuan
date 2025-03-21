using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWenJuan.Shared.Dtos;

public class OptionDto
{
	public int OptionId { get; set; }
	public int QuestionId { get; set; }
	public string OptionText { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
