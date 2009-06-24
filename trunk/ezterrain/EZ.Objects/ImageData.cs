using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OpenTK.Graphics;

namespace EZ.Objects
{
	public abstract class ImageData
	{
		protected ImageData(int pixelSize, int width, int height, int depth)
		{
			Buffer = new byte[pixelSize * width * height * depth];
			this.PixelSize = pixelSize;
			this.Width = width;
			this.Height = height;
			this.Depth = depth;
		}

		public byte[] Buffer { get; private set; }

		public int PixelSize { get; private set; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		public int Depth { get; private set; }
	}

	public abstract class ImageData<TPixel> : ImageData
		where TPixel : struct, IPixel
	{
		public static readonly new int PixelSize = Marshal.SizeOf(typeof(TPixel));

		protected ImageData(int width, int height, int depth)
			: base(PixelSize, width, height, depth)
		{ }

		protected TPixel GetPixel(int dataIndex)
		{
			TPixel pixel = new TPixel();
			pixel.CopyFrom(Buffer, dataIndex);

			return pixel;
		}

		protected void SetPixel(int dataIndex, TPixel value)
		{
			value.CopyTo(Buffer, dataIndex);
		}
	}
}
