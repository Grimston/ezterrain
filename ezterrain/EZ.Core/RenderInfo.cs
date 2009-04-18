using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Core
{
	public class RenderInfo
	{
		public RenderInfo(ViewerInfo viewer, FrameStamp frameStamp)
		{
			this.Viewer = viewer;
			this.FrameStamp = frameStamp;
		}

		public ViewerInfo Viewer { get; private set; }

		public FrameStamp FrameStamp { get; private set; }
	}
}
