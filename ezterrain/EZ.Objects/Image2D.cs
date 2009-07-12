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
			: base(target, new ImageData<TPixel>(width, height, 1))
		{ }

		protected override void Upload(Region3D region)
		{
			if (region == Bounds)
			{
				GL.TexImage2D(Target, 0, PixelInternalFormat, 
							  Data.Size.Width, Data.Size.Height, 0,
							  PixelFormat, PixelType, Data.Buffer);
			}
		}
	}
}
