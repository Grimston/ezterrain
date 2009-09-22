using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Imaging;
using OpenTK.Graphics;
using System.IO;

namespace EZ.Texturing
{
	public class TextureArray : BoundTexture
	{
		private HashSet<Region2D>[] dirtyRegions;

		public TextureArray(TextureUnit unit, ImageArray images)
			: base(unit, images)
		{
			dirtyRegions = new HashSet<Region2D>[images.Depth];

			for (int i = 0; i < dirtyRegions.Length; i++)
			{
				dirtyRegions[i] = new HashSet<Region2D>();
			}
		}

		private ImageArray ImageArray
		{
			get { return (ImageArray)Image; }
		}

		public override TextureTarget Target
		{
			get { return TextureTarget.Texture2DArray; }
		}

		protected override EnableCap? EnableCap
		{
			get { return OpenTK.Graphics.EnableCap.Texture3DExt; }
		}

		public void Dirty()
		{
			for (int i = 0; i < dirtyRegions.Length; i++)
			{
				Dirty(i);
			}
		}

		public void Dirty(int index)
		{
			dirtyRegions[index].Clear();
			dirtyRegions[index].Add(ImageArray[index].Bounds);
		}

		public void Dirty(int index, Region2D bounds)
		{
			dirtyRegions[index].Add(bounds);
		}

		private void Upload()
		{
			ITexImage texImage = Image as ITexImage;

			if (texImage != null)
			{
				texImage.Upload(Target, Image.Bounds);
			}
			else
			{
//				GL.TexImage3D(Target, 0, PixelInternalFormat.Rgb,
//								Image.Size.Width, Image.Size.Height, ((ImageArray)Image).Depth,
//								0, OpenTK.Graphics.PixelFormat.Bgr,
//								PixelType.UnsignedByte, IntPtr.Zero);
				GL.TexImage3D(Target, 0, PixelInternalFormat.R32f,
								Image.Size.Width, Image.Size.Height, ((ImageArray)Image).Depth,
								0, OpenTK.Graphics.PixelFormat.Red,
								PixelType.Float, IntPtr.Zero);
			}
		}

		public override void Update()
		{
			base.Update();

			if (!Initialized)
			{
				Upload();
				Dirty();
			}

			for (int i = 0; i < dirtyRegions.Length; i++)
			{
				ITexImage texImage = ImageArray[i] as ITexImage;

				if (texImage != null)
				{
					foreach (Region2D region in dirtyRegions[i])
					{
						texImage.Upload(Target, region);
					}
				}

				dirtyRegions[i].Clear();
			}
		}
	}
}
