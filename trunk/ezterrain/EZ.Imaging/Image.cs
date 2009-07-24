using System;
using System.Collections.Generic;
using EZ.Core;

namespace EZ.Imaging
{
	public abstract class Image
	{
		protected Image(Size2D size)
		{
			this.Size = size;
			this.Bounds = new Region2D(Index2D.Empty, this.Size);
		}

		public Size2D Size { get; private set; }

		public Region2D Bounds { get; set; }
	}
}
