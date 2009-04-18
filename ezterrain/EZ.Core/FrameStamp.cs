using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Core
{
	public struct FrameStamp
	{
		public static readonly FrameStamp Zero = new FrameStamp(0, 0, 0);

		public FrameStamp(uint frameNumber, float totalTime, float deltaTime)
		{
			FrameNumber = frameNumber;
			TotalTime = totalTime;
			DeltaTime = deltaTime;
		}

		public uint FrameNumber;

		public float TotalTime;

		public float DeltaTime;
	}
}
