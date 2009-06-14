using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Ez.Clipmaps
{
	public struct TmpCopyInfo
	{
		public TmpCopyInfo(Point offset1, Point offset2, Size size)
		{
			this.Offset1 = offset1;
			this.Offset2 = offset2;
			this.Size = size;
		}

		public Point Offset1;

		public Point Offset2;

		public Size Size;

		public Rectangle Rect1
		{
			get { return new Rectangle(Offset1, Size); }
		}

		public Rectangle Rect2
		{
			get { return new Rectangle(Offset2, Size); }
		}
	}
}
