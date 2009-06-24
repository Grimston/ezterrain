using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Objects
{
	public class Image3DData<TPixel> : ImageData<TPixel>
		where TPixel : struct, IPixel
	{
		public Image3DData(int width, int height, int depth)
			: base(width, height, depth)
		{ }

		public TPixel this[int column, int row, int depth]
		{
			get { return base[((depth * Height + row) * Width + column) * PixelSize]; }
			set { base[((depth * Height + row) * Width + column) * PixelSize] = value; }
		}
	}
}
