using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Imaging;
using OpenTK.Graphics;

namespace EZ.Objects
{
	public class Image3D<TPixel> : Image<TPixel>
		where TPixel : struct, IPixel
	{
		public Image3D(TextureTarget target, int width, int height, int depth)
			: base(target, new ImageData<TPixel>(width, height, depth))
		{ }

		protected override void Upload(Region3D region)
		{
			if (region == Bounds)
			{
				GL.TexImage3D(Target, 0, PixelInternalFormat,
							  Size.Width, Size.Height, Size.Depth, 0,
							  PixelFormat, PixelType, Buffer);
			}
			else
			{
				ImageData<TPixel> data = this[region];
				GL.TexSubImage3D(Target, 0, 
								 region.Column, region.Row, region.DepthIndex, 
								 region.Width, region.Height, region.Depth, 
								 PixelFormat, PixelType, data.Buffer);
			}
		}

		protected override Image<TPixel> NewImage(Size3D size)
		{
			return new Image3D<TPixel>(Target, size.Width, size.Height, size.Depth);
		}
	}
}
