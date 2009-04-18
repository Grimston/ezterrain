using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Math;

namespace EZ.Core
{
	public struct ViewerInfo
	{
		public ViewerInfo(Attitude attitude, Vector3 position, Perspective perspective)
		{
			this.attitude = attitude;
			this.position = position;
			this.perspective = perspective;
		}

		public ViewerInfo(Camera camera)
			: this(camera.Attitude, camera.Position, camera.Perspective)
		{ }

		private Attitude attitude;
		public Attitude Attitude
		{
			get { return attitude; }
		}

		private Vector3 position;
		public Vector3 Position
		{
			get { return position; }
		}

		private Perspective perspective;
		public Perspective Perspective
		{
			get { return perspective; }
		}
	}
}
