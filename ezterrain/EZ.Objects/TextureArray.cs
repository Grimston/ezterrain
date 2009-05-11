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
	public class TextureArray : BoundTexture
	{
		private static Bitmap DefaultBitmap { get { return new Bitmap(0, 0); } }

		public TextureArray(TextureUnit unit, Size size, Bitmap[] images)
			: base(unit, DefaultBitmap)
		{
			this.Size = size;
			LoadImages(images);
		}

		public void LoadImages(Bitmap[] images)
		{
			this.Images = new TextureArrayElement[images.Length];

			for (int i = 0; i < this.Images.Length; i++)
			{
				this.Images[i] = new TextureArrayElement(i, images[i]);
			}
		}

		public Size Size { get; private set; }

		public TextureArrayElement[] Images { get; private set; }

		public override TextureTarget Target
		{
			get { return TextureTarget.Texture2DArray; }
		}

		protected override EnableCap? EnableCap
		{
			get { return OpenTK.Graphics.EnableCap.Texture3DExt; }
		}

		public override void Initialize()
		{
			base.Initialize();

			Bind();

			GL.TexImage3D(Target, 0, PixelInternalFormat.Rgb,
							Size.Width, Size.Height, 6, 0, OpenTK.Graphics.PixelFormat.Bgr,
							PixelType.UnsignedByte, IntPtr.Zero);
		}

		protected override void Upload(BitmapData data)
		{
			Array.ForEach(Images, image => Update());
		}
	}
}
