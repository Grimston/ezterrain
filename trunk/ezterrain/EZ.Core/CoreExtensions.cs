using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Math;

namespace EZ.Core
{
	public static class CoreExtensions
	{
		public static string ToString(this Vector3 vector, uint decimals)
		{
			string format = "({0:f#}, {1:f#}, {2:f#})".Replace("#", decimals.ToString());
			return string.Format(format, vector.X, vector.Y, vector.Z);
		}

		public static string ToString(this Vector2 vector, uint decimals)
		{
			string format = "({0:f#}, {1:f#})".Replace("#", decimals.ToString());
			return string.Format(format, vector.X, vector.Y);
		}
	}
}
