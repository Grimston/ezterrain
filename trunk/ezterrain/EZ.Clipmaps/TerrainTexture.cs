using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Objects;
using OpenTK.Graphics;
using EZ.Core;
using EZ.Imaging;
using OpenTK.Math;
using EZ.Texturing;

namespace Ez.Clipmaps
{
	class TerrainTexture : TextureArray
	{
		#region Image loading
		private static Index2D GetInitialIndex(Image image, int windowSize)
		{
			return new Index2D(image.Size.Width / 2 - windowSize / 2,
								image.Size.Height / 2 - windowSize / 2);
		}

		private static ImageArray GetImageArray(string sourceFormat, int maxLevel, int size)
		{
			string format = ResourceManager.GetImagePath(sourceFormat);

			Size2D imageSize = new Size2D(size, size);

			TexImageArray imageArray = TextureHelper.GetImageArray(format, maxLevel, imageSize);

			for (int i = 0; i < imageArray.Depth; i++)
			{
				imageArray[i].Bounds = new Region2D(GetInitialIndex(imageArray[i], size), imageSize);
			}

			return imageArray;
		}
		#endregion

		public TerrainTexture(TextureUnit unit, int sideVertexCount, string sourceFormat, int maxLevels)
			: base(unit, GetImageArray(sourceFormat, maxLevels, sideVertexCount))
		{
			this.MinFilter = TextureMinFilter.Nearest;
			this.MagFilter = TextureMagFilter.Nearest;
			this.WrapS = TextureWrapMode.ClampToBorder;
			this.WrapT = TextureWrapMode.ClampToBorder;
		}

		public void SetEye(Vector3 eye)
		{
			ImageArray imageArray = (ImageArray)Image;
			for (int level = 0; level < imageArray.Depth; level++)
			{
				float levelScale = (float)(1 << (level + 1));
				Vector2 offset = eye.Xy / levelScale;

				Index2D index = GetInitialIndex(imageArray[level], Image.Size.Width);
				index.Offset((int)offset.X, (int)offset.Y);

				Region2D bounds = imageArray[level].Bounds;

				if (bounds.Index != index)
				{
					imageArray[level].Bounds = new Region2D(index, bounds.Size);
					Dirty(level);
				}
			}
		}
	}
}
