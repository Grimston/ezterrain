using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Objects
{
	public struct Index3D : IEquatable<Index3D>
	{
		public static readonly Index3D Empty = new Index3D();

		public Index3D(int column, int row, int depth)
		{
			this.Column = column;
			this.Row = row;
			this.Depth = depth;
		}

		public int Column;

		public int Row;

		public int Depth;

		public void Offset(Index3D other)
		{
			Column += other.Column;
			Row += other.Row;
			Depth += other.Depth;
		}

		public void Offset(int column, int row, int depth)
		{
			Column += column;
			Row += row;
			Depth += depth;
		}

		public static explicit operator Index2D(Index3D index)
		{
			return new Index2D(index.Column, index.Row);
		}

		public static bool operator ==(Index3D value1, Index3D value2)
		{
			return value1.Equals(value2);
		}

		public static bool operator !=(Index3D value1, Index3D value2)
		{
			return !value1.Equals(value2);
		}

		public override bool Equals(object obj)
		{
			return obj is Index3D && Equals((Index3D)obj);
		}

		public override int GetHashCode()
		{
			return Column ^ Row ^ Depth;
		}

		public bool Equals(Index3D other)
		{
			return Column == other.Column
				&& Row == other.Row
				&& Depth == other.Depth;
		}

		public override string ToString()
		{
			return string.Concat(Column, ", ", Row, ", ", Depth);
		}
	}
}
