using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Core
{
	public struct FrameStamp
	{
		public static readonly FrameStamp Zero = new FrameStamp(0, 0, 0);

		public FrameStamp(uint frameNumber, double totalTime, double deltaTime)
		{
			FrameNumber = frameNumber;
			TotalTime = totalTime;
			DeltaTime = deltaTime;
		}

		public uint FrameNumber;

		public double TotalTime;

		public double DeltaTime;
	}
}
