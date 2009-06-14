using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Ez.Clipmaps
{
	public struct CopyInfo
	{
		public CopyInfo(Point source, Point destination, Size size)
		{
			this.Source = source;
			this.Destination = destination;
			this.Size = size;
		}

		public Point Source;

		public Point Destination;

		public Size Size;

		public Rectangle SourceRect
		{
			get { return new Rectangle(Source, Size); }
		}

		public Rectangle DestinationRect
		{
			get { return new Rectangle(Destination, Size); }
		}
	}
}
