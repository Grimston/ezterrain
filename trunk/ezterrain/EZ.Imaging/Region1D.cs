using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Imaging
{
	public struct Region1D : IEquatable<Region1D>
	{
		public Region1D(int column, int width)
		{
			this.Column = column;
			this.Width = width;
		}

		public int Column;

		public int Width;

		public static bool operator==(Region1D value1, Region1D value2)
		{
			return value1.Equals(value2);
		}

		public static bool operator !=(Region1D value1, Region1D value2)
		{
			return !value1.Equals(value2);
		}

		public override bool Equals(object obj)
		{
			return obj is Region1D && Equals((Region1D)obj);
		}

		public override int GetHashCode()
		{
			return Column.GetHashCode() ^ Width.GetHashCode();
		}

		public bool Equals(Region1D other)
		{
			return Column == other.Column && Width == other.Width;
		}

		public override string ToString()
		{
			return string.Concat(Column, ", ", Width);
		}
	}
}
