using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Imaging;
using OpenTK.Graphics;

namespace EZ.Texturing
{
	public interface ITexImage
	{
		PixelInternalFormat InternalFormat { get; }

		PixelFormat Format { get; }

		PixelType Type { get; }

		void Upload(TextureTarget target, Region2D region);
	}
}
