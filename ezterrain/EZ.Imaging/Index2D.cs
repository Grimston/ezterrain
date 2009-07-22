using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Imaging
{
	public struct Index2D : IEquatable<Index2D>
	{
		public static readonly Index2D Empty = new Index2D(0, 0);
		
		public Index2D(int column, int row)
		{
			this.Column = column;
			this.Row = row;
		}

		public int Column;

		public int Row;

		public static bool operator ==(Index2D value1, Index2D value2)
		{
			return value1.Equals(value2);
		}

		public static bool operator !=(Index2D value1, Index2D value2)
		{
			return !value1.Equals(value2);
		}

		public override bool Equals(object obj)
		{
			return obj is Index2D && Equals((Index2D)obj);
		}

		public override int GetHashCode()
		{
			return Column ^ Row;
		}

		public bool Equals(Index2D other)
		{
			return Column == other.Column
				&& Row == other.Row;
		}
		
		public override string ToString()
		{
			return string.Concat(Column, ", ", Row);
		}
	}
}
