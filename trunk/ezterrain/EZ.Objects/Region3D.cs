using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Objects
{
	public struct Region3D : IEquatable<Region3D>
	{
		public Region3D(Index3D location, Size3D size)
		{
			this.Index = location;
			this.Size = size;
		}

		public Index3D Index;

		public Size3D Size;

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
