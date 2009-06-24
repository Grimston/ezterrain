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

		public static void Copy(Image1DData<TPixel> src, int srcColumn,
								Image1DData<TPixel> dst, int dstColumn,
								int width)
		{
			System.Buffer.BlockCopy(src.Buffer, srcColumn * PixelSize,
									dst.Buffer, dstColumn * PixelSize,
									width * PixelSize);
		}
	}
}
