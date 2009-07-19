using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Objects;
using OpenTK.Graphics;

namespace Ez.Clipmaps
{
	class TerrainTexture : TextureArray
	{
		#region GetImages
		private static TerrainImageHelper[] GetImageHelpers(string sourceName, int maxLevel, int size)
		{
			TerrainImageHelper[] imageHelpers = new TerrainImageHelper[maxLevel];

			for (int i = 0; i < maxLevel; i++)
			{
				IImage wholeImage = ImageHelper.GetArrayImage(sourceName + i, i);
				IImage gpuImage = wholeImage.NewImage(new Size3D(size, size, 1));
			}

			return imageHelpers;
		}

		private static IImage[] GetImages(TerrainImageHelper[] helpers)
		{
			IImage[] images = new IImage[helpers.Length];

			for (int i = 0; i < images.Length; i++, images[i] = helpers[i].GpuImage) ;

			return images;
		}
		#endregion

		private TerrainImageHelper[] helpers;

		public TerrainTexture(TextureUnit unit, int sideVertexCount, string sourceName, int maxLevel)
			: this(unit, sideVertexCount, GetImageHelpers(sourceName, maxLevel, sideVertexCount))
		{ }

		private TerrainTexture(TextureUnit unit, int size, TerrainImageHelper[] helpers)
			: base(unit, new Size2D(size, size), GetImages(helpers))
		{
			this.helpers = helpers;
		}
	}
}
