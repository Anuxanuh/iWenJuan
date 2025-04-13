using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWenJuan.Shared.Utils;
public static class StringOrNumericComparer
{
	public static int Compare(string a, string b)
	{
		if (double.TryParse(a, out var da) && double.TryParse(b, out var db))
		{
			return da.CompareTo(db);
		}
		else
		{
			return a.CompareTo(b);
		}
	}
}
