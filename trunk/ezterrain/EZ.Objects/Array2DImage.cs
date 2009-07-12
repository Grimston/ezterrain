using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;

namespace EZ.Objects
{
	public class Array2DImage<TPixel> : Image2D<TPixel>
		where TPixel: struct, IPixel
	{
		public Array2DImage(int index, int width, int height)
			: base(TextureTarget.Texture2DArray, width, height)
		{
			this.Index = index;
		}

		public Array2DImage(int index, ImageData<TPixel> data)
			: base(TextureTarget.Texture2DArray, data)
		{
			this.Index = index;
		}

		public int Index { get; private set; }

		protected override void Upload(Region3D region)
		{
			if (region == Bounds)
			{
				GL.TexSubImage3D(Target, 0,
								 0, 0, Index,
								 Size.Width, Size.Height, 1,
								 PixelFormat, PixelType, Buffer);
			}
			else
			{
				ImageData<TPixel> data = this[region];
				GL.TexSubImage3D(Target, 0,
								 region.Column, region.Row, Index,
								 region.Width, region.Height, 1,
								 PixelFormat, PixelType, data.Buffer);
			}
		}
	}
}
