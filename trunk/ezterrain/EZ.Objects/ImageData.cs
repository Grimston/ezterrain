using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OpenTK.Graphics;

namespace EZ.Objects
{
	public abstract class ImageData<TPixel>
		where TPixel : struct, IPixel
	{
		public static readonly int PixelSize = Marshal.SizeOf(typeof(TPixel));

		protected ImageData(int width, int height, int depth)
		{
			this.Width = width;
			this.Height = height;
			this.Depth = depth;
			this.Buffer = new TPixel[Width, Height, Depth];
		}

		protected TPixel[,,] Buffer { get; private set; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		public int Depth { get; private set; }
	}
}
