using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Objects
{
	public struct Region3D : IEquatable<Region3D>
	{
		public static Region3D FromLTNRBF(int left, int top, int near, int right, int bottom, int far)
		{
			return new Region3D(left, top, near, right - left, bottom - top, far - near);
		}

		public Region3D(int column, int row, int depthIndex, int width, int height, int depthSize)
		{
			this.Index = new Index3D(column, row, depthIndex);
			this.Size = new Size3D(width, height, depthSize);
		}

		public Region3D(Index3D index, Size3D size)
		{
			this.Index = index;
			this.Size = size;
		}

		public Index3D Index;

		public int Column 
		{
			get { return Index.Column; }
			set { Index.Column = value; }
		}

		public int Row
		{
			get { return Index.Row; }
			set { Index.Row = value; }
		}

		public int DepthIndex
		{
			get { return Index.Depth; }
			set { Index.Depth = value; }
		}

		public Size3D Size;

		public int Width
		{
			get { return Size.Width; }
			set { Size.Width = value; }
		}

		public int Height
		{
			get { return Size.Height; }
			set { Size.Height = value; }
		}

		public int Depth
		{
			get { return Size.Depth; }
			set { Size.Depth = value; }
		}

		public static explicit operator Region2D(Region3D region)
		{
			return new Region2D((Index2D)region.Index, (Size2D)region.Size);
		}

		public static bool operator==(Region3D value1, Region3D value2)
		{
			return value1.Equals(value2);
		}

		public static bool operator !=(Region3D value1, Region3D value2)
		{
			return !value1.Equals(value2);
		}

		public override bool Equals(object obj)
		{
			return obj is Region3D && Equals((Region3D)obj);
		}

		public override int GetHashCode()
		{
			return Index.GetHashCode() ^ Size.GetHashCode();
		}

		public bool Equals(Region3D other)
		{
			return Index.Equals(other.Index) && Size.Equals(other.Size);
		}
	}
}
