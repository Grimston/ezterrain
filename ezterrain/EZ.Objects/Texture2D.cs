﻿using System;
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

		protected override EnableCap? EnableCap
		{
			get { return OpenTK.Graphics.EnableCap.Texture2D; }
		}

		protected override void Upload(Rectangle region, BitmapData data)
		{
			if (data.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
			{
				if (Image.GetComponentCount(data.PixelFormat) == data.Stride / data.Width)
				{
					GL.TexImage2D(Target, 0, PixelInternalFormat.Rgb, data.Width, data.Height, 0,
									  OpenTK.Graphics.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
				}
				else
				{
					for (int i = 0; i < data.Height; i++)
					{
						GL.TexSubImage2D(Target, 0,
										 region.X, region.Y + i,
										 data.Width, 1,
										 OpenTK.Graphics.PixelFormat.Bgr, 
										 PixelType.UnsignedByte, 
										 new IntPtr(data.Scan0.ToInt32() + i * data.Stride));
					}
				}
			}
		}
	}
}