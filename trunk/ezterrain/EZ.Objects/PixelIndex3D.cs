using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Objects
{
	public struct PixelIndex3D
	{
		public PixelIndex3D(int column, int row, int depth)
		{
			this.column = column;
			this.row = row;
			this.depth = depth;
		}

		private int column;
		public int Column { get { return column; } }

		private int row;
		public int Row { get { return row; } }

		private int depth;
		public int Depth { get { return depth; } }
	}
}
