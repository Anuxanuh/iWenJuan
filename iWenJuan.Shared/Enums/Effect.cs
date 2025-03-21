using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWenJuan.Shared.Enums;

public enum Effect
{
	[Display(Name = "显示")]
	Show,
	[Display(Name = "隐藏")]
	Hide
}
