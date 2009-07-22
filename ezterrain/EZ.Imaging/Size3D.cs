using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Imaging
{
	public struct Size3D
	{
		public Size3D(int width, int height, int depth)
		{
			this.Width = width;
			this.Height = height;
			this.Depth = depth;
		}

		public int Width;

		public int Height;

		public int Depth;

		public static explicit operator Size2D(Size3D size)
		{
			return new Size2D(size.Width, size.Height);
		}

		public static bool operator ==(Size3D value1, Size3D value2)
		{
			return value1.Equals(value2);
		}

		public static bool operator !=(Size3D value1, Size3D value2)
		{
			return !value1.Equals(value2);
		}

		public override bool Equals(object obj)
		{
			return obj is Size3D && Equals((Size3D)obj);
		}

		public override int GetHashCode()
		{
			return Width ^ Height ^ Depth;
		}

		public bool Equals(Size3D other)
		{
			return Width == other.Width
				&& Height == other.Height
				&& Depth == other.Depth;
		}

		public override string ToString()
		{
			return string.Concat(Width, ", ", Height, ", ", Depth);
		}
	}
}
