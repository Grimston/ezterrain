using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EZ.Core
{
	public static class ResourceManager
	{
		private static readonly string up = ".." + Path.DirectorySeparatorChar;
		private static readonly string current = "." + Path.DirectorySeparatorChar;
		
		public static readonly string ResourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
		                                                          up + up + up + "Resources");

		public static string GetProgramPath(string fileName)
		{
			return Path.Combine(Path.Combine(ResourcePath, current + "Programs"), fileName);
		}

		public static string GetImagePath(string fileName)
		{
			return Path.Combine(Path.Combine(ResourcePath, current + "Images"), fileName);
		}
	}
}
