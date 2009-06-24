using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Objects
{
	public class GlImage1D<TPixel> : GlImage<Image1DData<TPixel>, TPixel>
		where TPixel : struct, IPixel
	{
		public GlImage1D(int width)
			: base(new Image1DData<TPixel>(width))
		{ }
	}

	public class GlImage2D<TPixel> : GlImage<Image2DData<TPixel>, TPixel>
		where TPixel : struct, IPixel
	{
		public GlImage2D(int width, int height)
			: base(new Image2DData<TPixel>(width, height))
		{ }
	}

	public class GlImage3D<TPixel> : GlImage<Image3DData<TPixel>, TPixel>
		where TPixel : struct, IPixel
	{
		public GlImage3D(int width, int height, int depth)
			: base(new Image3DData<TPixel>(width, height, depth))
		{ }
	}
}
