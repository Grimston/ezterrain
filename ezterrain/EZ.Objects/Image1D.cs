using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Imaging;

using OpenTK.Graphics;

namespace EZ.Objects
{
	public class Image1D<TPixel> : Image<TPixel>
		where TPixel : struct, IPixel
	{
		public Image1D(TextureTarget target, int width)
			: base(target, new ImageData<TPixel>(width, 1, 1))
		{ }

		protected override void Upload(Region3D region)
		{
			if (region == Bounds)
			{
				GL.TexImage1D(Target, 0, PixelInternalFormat, Size.Width, 0, PixelFormat, PixelType, Buffer);
			}
			else
			{
				ImageData<TPixel> data = this[region];
				GL.TexSubImage1D(Target, 0, region.Column, region.Width, PixelFormat, PixelType, data.Buffer);
			}
		}

		protected override Image<TPixel> NewImage(Size3D size)
		{
			return new Image1D<TPixel>(Target, size.Width);
		}
	}
}
