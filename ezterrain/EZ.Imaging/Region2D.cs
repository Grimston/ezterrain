using System;


namespace EZ.Imaging
{
	public struct Region2D: IEquatable<Region2D>
	{
		public Region2D(Index2D location, Size2D size)
		{
			this.Index = location;
			this.Size = size;
		}

		public Index2D Index;

		public Size2D Size;

		public static explicit operator Region1D(Region2D region)
		{
			return new Region1D(region.Index.Column, region.Size.Width);
		}

		public static bool operator==(Region2D value1, Region2D value2)
		{
			return value1.Equals(value2);
		}

		public static bool operator !=(Region2D value1, Region2D value2)
		{
			return !value1.Equals(value2);
		}

		public override bool Equals(object obj)
		{
			return obj is Region2D && Equals((Region2D)obj);
		}

		public override int GetHashCode()
		{
			return Index.GetHashCode() ^ Size.GetHashCode();
		}

		public bool Equals(Region2D other)
		{
			return Index.Equals(other.Index) && Size.Equals(other.Size);
		}

		public override string ToString()
		{
			return string.Concat('(',Index, "), (", Size, ")");
		}
	}
}
