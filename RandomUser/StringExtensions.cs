using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUser
{
	public static class StringExtensions
	{
		public static string StripSpacesToLower(this string source)
		{
			return source.Replace(" ", "").ToLower();
		}
	}
}
