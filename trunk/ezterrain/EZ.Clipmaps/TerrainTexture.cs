using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Objects;
using OpenTK.Graphics;
using EZ.Core;
using OpenTK.Math;

namespace Ez.Clipmaps
{
	class TerrainTexture : TextureArray
	{
		#region GetImages
		private static TerrainImageHelper[] GetImageHelpers(string sourceFormat, int maxLevel, int size)
		{
			string format = ResourceManager.GetImagePath(sourceFormat);
			TerrainImageHelper[] helpers = new TerrainImageHelper[maxLevel];

			for (int i = 0; i < maxLevel; i++)
			{
				IImage wholeImage = ImageHelper.GetArrayImage(string.Format(format, i), i);
				IImage gpuImage = wholeImage.NewImage(new Size3D(GetExpanded(size), GetExpanded(size), 1));

				helpers[i] = new TerrainImageHelper(wholeImage, gpuImage);
			}

			return helpers;
		}

		private static IImage[] GetImages(TerrainImageHelper[] helpers)
		{
			IImage[] images = new IImage[helpers.Length];

			for (int i = 0; i < images.Length; images[i] = helpers[i].GpuImage, i++) ;

			return images;
		}

		private static int GetExpanded(int size)
		{
			return (size >> 1) << 2;
		}
		#endregion

		private TerrainImageHelper[] helpers;

		public TerrainTexture(TextureUnit unit, int sideVertexCount, string sourceFormat, int maxLevels)
			: this(unit, sideVertexCount, GetImageHelpers(sourceFormat, maxLevels, sideVertexCount))
		{ }

		private TerrainTexture(TextureUnit unit, int size, TerrainImageHelper[] helpers)
			: base(unit, new Size2D(GetExpanded(size), GetExpanded(size)), GetImages(helpers))
		{
			this.helpers = helpers;
			this.MinFilter = TextureMinFilter.Nearest;
			this.MagFilter = TextureMagFilter.Nearest;
			this.WrapS = TextureWrapMode.ClampToBorder;
			this.WrapT = TextureWrapMode.ClampToBorder;
		}

		public void SetEye(Vector3 eye)
		{
			for (int level = 0; level < helpers.Length; level++)
			{
				float levelScale = (float)(1 << (level+1));
				Vector2 offset = eye.Xy / levelScale;

				helpers[level].Copy((int)offset.X, (int)offset.Y);
			}
		}
	}
}
