using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Math;

namespace EZ.Core
{
	public class Camera
	{
		public Camera()
		{
			Attitude = Attitude.ENU;
			Position = Vector3.Zero;
			Perspective = Perspective.Default;
		}

		public Attitude Attitude { get; set; }

		public Vector3 Position { get; set; }

		public Perspective Perspective { get; set; }
	}
}
