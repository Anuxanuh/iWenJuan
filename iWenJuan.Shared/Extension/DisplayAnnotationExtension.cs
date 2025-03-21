using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace iWenJuan.Shared.Extension;

public static class DisplayAnnotationExtension
{
	public static string GetDisplayName(this Enum enumValue)
	{
		var displayAttribute = enumValue.GetType()
			.GetField(enumValue.ToString())?
			.GetCustomAttribute<DisplayAttribute>();

		return displayAttribute?.Name ?? enumValue.ToString();
	}
}
