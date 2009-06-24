using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Objects
{
	public struct PixelIndex2D
	{
		public PixelIndex2D(int column, int row)
		{
			this.column = column;
			this.row = row;
		}

		private int column;
		public int Column { get { return column; } }

		private int row;
		public int Row { get { return row; } }
	}
}
