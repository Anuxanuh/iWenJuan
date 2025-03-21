using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWenJuan.Shared.Enums;

public enum IWenJuanChartType
{
	[Display(Name = "面积图")]
	Area,
	[Display(Name = "条形图")]
	Bar,
	[Display(Name = "柱状图")]
	Column,
	[Display(Name = "环形图")]
	Donut,
	[Display(Name = "折线图")]
	Line,
	[Display(Name = "饼图")]
	Pie,
	[Display(Name = "堆叠面积图")]
	StackedArea,
	[Display(Name = "堆叠条形图")]
	StackedBar,
	[Display(Name = "堆叠柱状图")]
	StackedColumn
}
