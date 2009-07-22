using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Imaging;

namespace EZ.Objects
{
	public struct CopyInfo
	{
		public CopyInfo(Index3D source, Index3D destination, Size3D size)
		{
			this.Source = source;
			this.Destination = destination;
			this.Size = size;
		}

		public Index3D Source;

		public Index3D Destination;

		public Size3D Size;

		public Region3D SourceRegion
		{
			get { return new Region3D(Source, Size); }
		}

		public Region3D DestinationRegion
		{
			get { return new Region3D(Destination, Size); }
		}
	}
}
