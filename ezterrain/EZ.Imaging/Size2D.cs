using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Imaging
{
	public struct Size2D : IEquatable<Size2D>
	{
		public Size2D(int width, int height)
		{
			this.Width = width;
			this.Height = height;
		}

		public int Width;

		public int Height;

		public static bool operator ==(Size2D value1, Size2D value2)
		{
			return value1.Equals(value2);
		}

		public static bool operator !=(Size2D value1, Size2D value2)
		{
			return !value1.Equals(value2);
		}

		public override bool Equals(object obj)
		{
			return obj is Size2D && Equals((Size2D)obj);
		}

		public override int GetHashCode()
		{
			return Width ^ Height;
		}

		public bool Equals(Size2D other)
		{
			return Width == other.Width
				&& Height == other.Height;
		}

		public override string ToString()
		{
			return string.Concat(Width, ", ", Height);
		}
	}
}
