using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
							  Data.Size.Width, Data.Size.Height, Data.Size.Depth, 0,
							  PixelFormat, PixelType, Data.Buffer);
			}
		}
	}
}
