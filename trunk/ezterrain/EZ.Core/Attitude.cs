using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Math;

namespace EZ.Core
{
	public struct Attitude
	{
		public static readonly Attitude ENU = new Attitude(Vector3.UnitY, Vector3.UnitZ);
		public static readonly Attitude NED = new Attitude(Vector3.UnitX, -Vector3.UnitZ);

		public Attitude(Vector3 direction, Vector3 up)
		{
			this.Direction = direction;
			this.Up = up;
		}

		public Vector3 Up;
		public Vector3 Direction;

		public Vector3 Side
		{
			get { return Vector3.Cross(Direction, Up); }
		}

		public Matrix4 ToRotation()
		{
			Vector3 side = this.Side;
			Vector3 up = Vector3.Cross(side, Direction);

			Matrix4 lookMatrix = new Matrix4(new Vector4(side, 0), 
											 new Vector4(up, 0),
											 new Vector4(-Direction, 0),
											 new Vector4(0, 0, 0, 1));

			return lookMatrix;
		}
	}
}
