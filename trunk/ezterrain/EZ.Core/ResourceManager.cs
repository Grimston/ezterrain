using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EZ.Core
{
	public static class ResourceManager
	{
		public static readonly string ResourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Resources");

		public static string GetProgramPath(string fileName)
		{
			return Path.Combine(Path.Combine(ResourcePath, @".\Programs"), fileName);
		}

		public static string GetImagePath(string fileName)
		{
			return Path.Combine(Path.Combine(ResourcePath, @".\Images"), fileName);
		}
	}
}
