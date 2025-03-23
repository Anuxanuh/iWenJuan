using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWenJuan.Shared.Enums;
public enum AggregateOperationType
{
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
