using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWenJuan.Shared.Extension;

public static class ExpandoObjectExtension
{
	public static bool CanConvertToDouble(this IDictionary<string, string> expando, string propertyName)
	{
		if (expando.TryGetValue(propertyName, out var value))
		{
			if (double.TryParse(value, out _))
			{
				return true;
			}
		}
		return false;
	}
}
