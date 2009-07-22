using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Imaging;

namespace EZ.Imaging
{
	public struct CopyInfo
	{
		public CopyInfo(Index2D source, Index2D destination, Size2D size)
		{
			this.Source = source;
			this.Destination = destination;
			this.Size = size;
		}

		public Index2D Source;

		public Index2D Destination;

		public Size2D Size;

		public Region2D SourceRegion
		{
			get { return new Region2D(Source, Size); }
		}

		public Region2D DestinationRegion
		{
			get { return new Region2D(Destination, Size); }
		}
	}
}
