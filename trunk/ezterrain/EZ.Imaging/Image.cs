using System;
using System.Collections.Generic;
using EZ.Core;

namespace EZ.Imaging
{
	public abstract class Image
	{
		private HashSet<Region2D> dirtyRegions;
		
		public Image(Size2D size)
		{
			this.Size = size;
			dirtyRegions = new HashSet<Region2D>();
		}
		
		public Size2D Size { get; private set; }
		
		protected Region2D Bounds
		{
			get { return new Region2D(Index2D.Empty, Size); }
		}
		
		public void Dirty()
		{
			dirtyRegions.Clear();
			dirtyRegions.Add(Bounds);
		}
		
		public void Dirty(Region2D region)
		{
			//TODO: check for region inclusion
			dirtyRegions.Add(region);
		}

		public abstract PinnedImage Pin();
	}
}
