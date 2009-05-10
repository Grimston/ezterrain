using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace EZ.Objects
{
	public class Texture2D : BoundTexture
	{
		public Texture2D(TextureUnit unit, string fileName)
			: base(unit, fileName)
		{ }

		public Texture2D(TextureUnit unit, Stream stream)
			: base(unit, stream)
		{ }

		public Texture2D(TextureUnit unit, Bitmap bitmap)
			: base(unit, bitmap)
		{ }

		public override TextureTarget Target
		{
			get { return TextureTarget.Texture2D; }
		}

		protected override void Upload(BitmapData data)
		{
			if (data.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
			{
				GL.TexImage2D(Target, 0, PixelInternalFormat.Rgb, data.Width, data.Height, 0,
								  OpenTK.Graphics.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
			}
		}
	}
}
