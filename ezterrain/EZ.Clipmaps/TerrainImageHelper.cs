using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Objects;

namespace Ez.Clipmaps
{
	class TerrainImageHelper
	{
		private Index3D center;

		public TerrainImageHelper(IImage wholeImage, IImage gpuImage)
		{
			this.WholeImage = wholeImage;
			this.center = new Index3D(wholeImage.Size.Width / 2, wholeImage.Size.Height / 2, 0);

			this.GpuImage = gpuImage;
			this.center.Offset(-gpuImage.Size.Width / 2, -gpuImage.Size.Height / 2, 0);

			Copy(0, 0);
		}

		public void Copy(int xOffset, int yOffset)
		{
			Index3D center = this.center;
			center.Offset(xOffset, yOffset, 0);

			WholeImage.CopyTo(GpuImage, new CopyInfo(center, Index3D.Empty, GpuImage.Size));
		}

		public IImage GpuImage { get; private set; }
		public IImage WholeImage { get; private set; }
	}
}
