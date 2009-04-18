using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Core
{
	public struct Perspective
	{
		public static readonly Perspective Default = new Perspective(90, 1, 10000);

		public Perspective(float angle, float near, float far)
		{
			this.Angle = angle;
			this.Near = near;
			this.Far = far;
		}

		public float Angle;
		public float Near;
		public float Far;
	}
}
