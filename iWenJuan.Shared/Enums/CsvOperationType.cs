using System.ComponentModel.DataAnnotations;

namespace iWenJuan.Shared.Enums;

public enum CsvOperationType
{
	[Display(Name = "选择")]
	Select,
	[Display(Name = "过滤")]
	Filter,
	[Display(Name = "分组")]
	GroupBy,
	[Display(Name = "排序")]
	OrderBy,
	[Display(Name = "计数")]
	Count,
	[Display(Name = "求和")]
	Sum,
	[Display(Name = "平均值")]
	Average,
	[Display(Name = "最小值")]
	Min,
	[Display(Name = "最大值")]
	Max,
}
