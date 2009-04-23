using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using System.Drawing;
using System.Drawing.Imaging;

namespace EZ.Objects
{
	public class Texture : Disposable
	{
		public Texture(string fileName)
			: this(new Bitmap(fileName))
		{ }

		public Texture(Bitmap bitmap)
		{
			this.Bitmap = bitmap;
			this.Target = TextureTarget.Texture2D;
		}

		public Bitmap Bitmap { get; private set; }

		public bool Initialized { get; private set; }

		public int Handle { get; private set; }

		public TextureTarget Target { get; set; }

		public void Initialize()
		{
			if (!Initialized)
			{
				Handle = GL.GenTexture();

				Bind();

				if (Bitmap != null)
				{
					UploadData(false);
				}

				GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

				int mipMapCount;
				GL.GetTexParameter(Target, GetTextureParameter.TextureMaxLevel, out mipMapCount);

				if (mipMapCount == 0)
				{// if no MipMaps are present, use linear Filter
					GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				}
				else
				{// MipMaps are present, use trilinear Filter
					GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
				}

				Initialized = true;
			}
		}

		public void Bind()
		{
			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(Target, Handle);
		}

		public void UploadData(bool bind)
		{
			if (bind)
			{
				Bind();
			}

			Rectangle rect = new Rectangle(0, 0, Bitmap.Width, Bitmap.Height);
			BitmapData data = Bitmap.LockBits(rect, ImageLockMode.ReadOnly,
											  System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			
			GL.TexImage2D(Target, 0, PixelInternalFormat.Rgb, data.Width, data.Height, 0,
						  OpenTK.Graphics.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);

			Bitmap.UnlockBits(data);
		}

		protected override void Dispose(bool nongc)
		{
			if (nongc && Initialized)
			{
				GL.DeleteTexture(Handle);

				Initialized = false;
				Handle = 0;
			}
		}
	}
}
