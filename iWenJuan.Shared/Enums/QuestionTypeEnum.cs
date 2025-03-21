using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace iWenJuan.Shared.Enums;

public enum QuestionTypeEnum
{
	[Display(Name = "单选题")]
	SingleChoice,
	[Display(Name = "多选题")]
	MultipleChoice,
	[Display(Name = "NPS")]
	Nps,
	[Display(Name = "单行文本")]
	SingleText,
	[Display(Name = "多行文本")]
	MultipleText,
	[Display(Name = "日期时间")]
	Datetime
}
