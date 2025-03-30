using System.ComponentModel.DataAnnotations;

namespace iWenJuan.Shared.Enums;

public enum CsvOperationType
{
	[Display(Name = "选择")]
	Select,
	[Display(Name = "过滤")]
	Filter,
	[Display(Name = "排序")]
	OrderBy,
	[Display(Name = "降序排序")]
	OrderByDescending,
	[Display(Name = "分组")]
	GroupBy,
	[Display(Name = "聚合")]
	Aggregate,
}
