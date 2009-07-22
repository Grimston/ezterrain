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
		}
		
		public Size2D Size { get; private set; }
		
		protected Region2D Bounds
		{
			get { return new Region2D(Index2D.Empty, Size); }
		}
	}
}
