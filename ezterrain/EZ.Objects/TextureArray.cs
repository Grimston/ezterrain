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
	public class TextureArray : Disposable
	{
		public TextureArray(TextureUnit unit)
		{
			this.Unit = unit;
			this.Target = TextureTarget.Texture2DArray;
		}

		public TextureArray(TextureUnit unit, Bitmap[] images)
			: this(unit)
		{
			LoadImages(images);
		}

		public void LoadImages(IEnumerable<string> files)
		{
			LoadImages(files.Select(file => new Bitmap(file)));
		}

		public void LoadImages(IEnumerable<Bitmap> images)
		{
			this.Images = images.ToArray();
		}

		public TextureUnit Unit { get; set; }

		public bool Initialized { get; private set; }

		public int Handle { get; private set; }

		public TextureTarget Target { get; private set; }

		public Bitmap[] Images { get; private set; }

		public void Initialize()
		{
			if (!Initialized)
			{
				Handle = GL.GenTexture();

				Bind();

				GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
				GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
				GL.TexParameter(Target, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
				GL.TexParameter(Target, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
				GL.TexParameter(Target, TextureParameterName.GenerateMipmapSgis, (int)All.False);

				GL.TexImage3D(Target, 0, PixelInternalFormat.Rgb,
								257, 257, 6, 0, OpenTK.Graphics.PixelFormat.Bgr,
								PixelType.UnsignedByte, IntPtr.Zero);

				if (Images != null)
				{
					UploadData(false);
				}

				Initialized = true;
			}
		}

		public void Bind()
		{
			GL.Enable(EnableCap.Texture3DExt);
			GL.ActiveTexture(Unit);
			GL.BindTexture(Target, Handle);
		}

		public void Unbind()
		{
			GL.ActiveTexture(Unit);
			GL.BindTexture(Target, 0);
			GL.Disable(EnableCap.Texture3DExt);
		}

		public void UploadData(bool bind, int imageIndex, Rectangle region)
		{
			if (bind)
			{
				Bind();
			}

			Bitmap image = Images[imageIndex];

			BitmapData data = image.LockBits(region, ImageLockMode.ReadOnly,
											 System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			GL.TexSubImage3D(TextureTarget.Texture2DArray, 0,
							 0, 0, 0,
							 data.Width, data.Height, 1,
							 OpenTK.Graphics.PixelFormat.Bgr,
							 PixelType.UnsignedByte,
							 data.Scan0);

			image.UnlockBits(data);
		}

		public void UploadData(bool bind, int imageIndex)
		{
			UploadData(bind, imageIndex, new Rectangle(0, 0, Images[imageIndex].Width, Images[imageIndex].Height));
		}

		public void UploadData(bool bind)
		{
			if (bind)
			{
				Bind();
			}

			for (int i = 0; i < Images.Length; i++)
			{
				UploadData(false, i);
			}
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
