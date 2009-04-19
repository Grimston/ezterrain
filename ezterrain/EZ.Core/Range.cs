using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Core
{
	public struct Range
	{
		public static readonly Range Invalid = new Range(int.MaxValue, int.MinValue);

		public Range(int min, int max)
		{
			Min = min;
			Max = max;
		}

		public bool IsValid
		{
			get
			{
				return Min <= Max;
			}
		}

		public int Min;

		public int Max;

		public int Length
		{
			get { return Max - Min + 1; }
		}

		public void Expand(int value)
		{
			if (value < Min)
			{
				Min = value;
			}

			if (value > Max)
			{
				Max = value;
			}
		}

		public void Expand(Range other)
		{
			if (other.IsValid)
			{
				if (other.Min < Min)
				{
					Min = other.Min;
				}

				if (other.Max > Max)
				{
					Max = other.Max;
				}
			}
		}
	}
}
