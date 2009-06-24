using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Objects
{
	public class Image1DData<TPixel> : ImageData<TPixel>
		where TPixel : struct, IPixel
	{
		public Image1DData(int width)
			: base(width, 1, 1)
		{ }

		public TPixel this[int column]
		{
			get { return GetPixel(column * PixelSize); }
			set { SetPixel(column * PixelSize, value); }
		}
	}
}
