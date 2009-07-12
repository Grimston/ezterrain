using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;

namespace EZ.Objects
{
	public class Image2D<TPixel> : Image<TPixel>
		where TPixel : struct, IPixel
	{
		public Image2D(TextureTarget target, int width, int height)
			: this(target, new ImageData<TPixel>(width, height, 1))
		{ }

		public Image2D(TextureTarget target, ImageData<TPixel> data)
			: base(target, data)
		{
			if (data.Size.Depth > 1)
			{
				throw new ArgumentException("Data depth should be 1", "data");
			}
		}

		protected override void Upload(Region3D region)
		{
			if (region == Bounds)
			{
				GL.TexImage2D(Target, 0, PixelInternalFormat,
							  Size.Width, Size.Height, 0,
							  PixelFormat, PixelType, Buffer);
			}
			else
			{
				ImageData<TPixel> data = this[region];
				GL.TexSubImage2D(Target, 0, 
								 region.Column, region.Row, 
								 region.Width, region.Height, 
								 PixelFormat, PixelType, data.Buffer);
			}
		}
	}
}
