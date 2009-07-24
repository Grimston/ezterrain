using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Imaging;
using OpenTK.Graphics;

namespace EZ.Texturing
{
	public class TexImage2D<T> : Image2D<T>, ITexImage
		where T : struct, IPixel
	{
		public TexImage2D(Size2D size)
			: base(size)
		{ }

		public TexImage2D(T[,] pixels)
			: base(pixels)
		{ }

		public void Upload(TextureTarget target, Region2D region)
		{
			GL.PixelStore(PixelStoreParameter.UnpackRowLength, Size.Width);
			GL.PixelStore(PixelStoreParameter.UnpackSkipRows, Bounds.Index.Row);
			GL.PixelStore(PixelStoreParameter.UnpackSkipPixels, Bounds.Index.Column);

			UploadImage(target, region);

			GL.PixelStore(PixelStoreParameter.UnpackSkipRows, 0);
			GL.PixelStore(PixelStoreParameter.UnpackSkipPixels, 0);
		}

		protected virtual void UploadImage(TextureTarget target, Region2D region)
		{
			if (region == Bounds)
			{
				GL.TexImage2D(target, 0, InternalFormat,
								region.Size.Width, region.Size.Width, 0,
								Format, Type, Pixels);
			}
			else
			{
				GL.TexSubImage2D(target, 0, region.Index.Column, region.Index.Row,
									region.Size.Width, region.Size.Width,
									Format, Type, Pixels);
			}
		}

		public PixelInternalFormat InternalFormat
		{
			get { return Pixel<T>.PixelInternalFormat; }
		}

		public PixelFormat Format
		{
			get { return Pixel<T>.PixelFormat; }
		}

		public PixelType Type
		{
			get { return Pixel<T>.PixelType; }
		}
	}
}
