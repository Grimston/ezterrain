using System;
using System.Runtime.InteropServices;

namespace EZ.Imaging
{
	public class Image2D<T> : Image
		where T: struct, IPixel
	{
		public Image2D(Size2D size)
			: base(size)
		{
			Pixels = new T[size.Height, size.Width];
		}

		public Image2D(T[,] pixels)
			: base(new Size2D(pixels.GetLength(1), pixels.GetLength(0)))
		{
			this.Pixels = pixels;
		}
		
		public T[,] Pixels { get; private set; }
	}
}
