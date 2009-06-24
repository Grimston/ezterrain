using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Objects
{
	public struct Size2D
	{
		public Size2D(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		private int width;
		public int Width { get { return width; } }

		private int height;
		public int Height { get { return height; } }
	}
}
