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
			get { return GetPixel(GetIndex(column, row)); }
			set { SetPixel(GetIndex(column, row), value); }
		}

		private int GetIndex(int column, int row)
		{
			return (row * Width + column) * PixelSize;
		}

		public static void Copy(Image2DData<TPixel> src, PixelIndex2D srcIndex,
								Image2DData<TPixel> dst, PixelIndex2D dstIndex,
								Size2D copySize)
		{
			int srcRowStride = src.Width * PixelSize;
			int dstRowStride = dst.Width * PixelSize;

			for (int row = 0; row < copySize.Height; row++)
			{
				int srcOffset = (srcIndex.Row + row) * srcRowStride
								+ srcIndex.Column * PixelSize;
				int dstOffset = (dstIndex.Row + row) * dstRowStride
								+ dstIndex.Column * PixelSize;

				System.Buffer.BlockCopy(src.Buffer, srcOffset,
										dst.Buffer, dstOffset,
										copySize.Width * PixelSize);
			}
		}
	}
}
