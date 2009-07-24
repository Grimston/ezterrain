using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Imaging;
using OpenTK.Graphics;

namespace EZ.Texturing
{
	public class TexImageArray : ImageArray, ITexImage
	{
		public TexImageArray(Size2D size, params Image[] images)
			: base(size, images)
		{ }

		public void Upload(TextureTarget target, Region2D region)
		{
			GL.TexImage3D(target, 0, InternalFormat, 
							Size.Width, Size.Height, Depth, 
							0, Format, Type, 
							IntPtr.Zero);
		}

		public PixelInternalFormat InternalFormat
		{
			get { return ((ITexImage)this[0]).InternalFormat; }
		}

		public PixelFormat Format
		{
			get { return ((ITexImage)this[0]).Format; }
		}

		public PixelType Type
		{
			get { return ((ITexImage)this[0]).Type; }
		}
	}
}
