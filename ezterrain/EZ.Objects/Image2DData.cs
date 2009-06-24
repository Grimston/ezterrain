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
	}
}
