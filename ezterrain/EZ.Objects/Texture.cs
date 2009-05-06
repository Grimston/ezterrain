using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace EZ.Objects
{
	public class Texture : Disposable
	{
		public Texture(TextureUnit unit, string fileName)
			: this(unit, new Bitmap(fileName))
		{ }

		public Texture(TextureUnit unit, Stream stream)
			: this(unit, new Bitmap(stream))
		{ }

		public Texture(TextureUnit unit, Bitmap bitmap)
		{
			this.Unit = unit;
			this.Bitmap = bitmap;
			this.Target = TextureTarget.Texture2D;
		}

		public TextureUnit Unit { get; set; }

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

				int mipMapCount;
				GL.GetTexParameter(Target, GetTextureParameter.TextureMaxLevel, out mipMapCount);

				if (Bitmap != null)
				{
					UploadData(false);
				}

				GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
				GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);

				Initialized = true;
			}
		}

		public void Bind()
		{
			GL.Enable(EnableCap.Texture2D);
			GL.ActiveTexture(Unit);
			GL.BindTexture(Target, Handle);
		}

		public void Unbind()
		{
			GL.ActiveTexture(Unit);
			GL.BindTexture(Target, 0);
			GL.Disable(EnableCap.Texture2D);
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
