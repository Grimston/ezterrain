using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Imaging;
using OpenTK.Graphics;

namespace EZ.Texturing
{
	public class TexArrayImage2D<T> : TexImage2D<T>
		where T: struct, IPixel
	{
		public TexArrayImage2D(int index, Size2D size)
			: base(size)
		{
			this.Index = index;
		}

		public TexArrayImage2D(int index, T[,] pixels)
			: base(pixels)
		{
			this.Index = index;
		}

		public int Index { get; private set; }

		protected override void UploadImage(TextureTarget target, Region2D region)
		{
			GL.TexSubImage3D(target, 0,
							 region.Index.Column - Bounds.Index.Column, region.Index.Row - Bounds.Index.Row, Index,
							 region.Size.Width, region.Size.Height, 1,
							 Pixel<T>.PixelFormat, Pixel<T>.PixelType, Pixels);
		}
	}
}
