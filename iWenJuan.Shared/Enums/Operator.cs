using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWenJuan.Shared.Enums;


public enum Operator
{
	[Display(Name = "==")]
	Equals,
	[Display(Name = "!=")]
	NotEquals,
	[Display(Name = "包含")]
	Contains,
	[Display(Name = ">")]
	GreaterThan,
	[Display(Name = "<")]
	LessThan,
	[Display(Name = ">=")]
	GreaterThanOrEquals,
	[Display(Name = "<=")]
	LessThanOrEquals,
}
