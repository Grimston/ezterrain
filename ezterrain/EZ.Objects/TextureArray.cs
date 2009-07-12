using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using System.IO;

namespace EZ.Objects
{
	public class TextureArray : BoundTexture
	{
		private static readonly IImage DefaultImage = new EmptyImage();

		public TextureArray(TextureUnit unit, Size2D size, IImage[] images)
			: base(unit, DefaultImage)
		{
			this.Size = size;
			LoadImages(images);
		}

		public void LoadImages(IImage[] images)
		{
			this.Images = new TextureArrayElement[images.Length];

			for (int i = 0; i < this.Images.Length; i++)
			{
				this.Images[i] = new TextureArrayElement(i, images[i]);
			}
		}

		public Size2D Size { get; private set; }

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

		public override void Update()
		{
			base.Update();

			foreach (TextureArrayElement element in Images)
			{
				element.Update();
			}
		}

		//protected override void Upload(Rectangle region, BitmapData data)
		//{
		//    Array.ForEach(Images, image => image.Update());
		//}
	}
}
