using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Objects
{
	public class Image2DData<TPixel> : ImageData<TPixel>
		where TPixel : struct, IPixel
	{
		public Image2DData(int width, int height)
			: base(width, height, 1)
		{ }

		public TPixel this[int column, int row]
		{
			get { return base[(row * Height + column) * PixelSize]; }
			set { base[(row * Height + column) * PixelSize] = value; }
		}
	}
}
