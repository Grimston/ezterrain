using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Objects;

namespace Ez.Clipmaps
{
	public struct TmpCopyInfo
	{
		public TmpCopyInfo(Index3D offset1, Index3D offset2, Size3D size)
		{
			this.Offset1 = offset1;
			this.Offset2 = offset2;
			this.Size = size;
		}

		public Index3D Offset1;

		public Index3D Offset2;

		public Size3D Size;

		public Region3D Region1
		{
			get { return new Region3D(Offset1, Size); }
		}

		public Region3D Region2
		{
			get { return new Region3D(Offset2, Size); }
		}
	}
}
