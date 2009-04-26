using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Math;
using System.Runtime.InteropServices;

namespace Ez.Clipmaps
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexP
	{
		public VertexP(Vector3 position)
		{
			this.Position = position;
		}

		public VertexP(float x, float y, float z)
		{
			this.Position = new Vector3(x, y, z);
		}

		public Vector3 Position;

		public override string ToString()
		{
			return string.Format("P{0}", Position);
		}

		public static readonly int SizeInBytes = Marshal.SizeOf(typeof(VertexPT));
		public static readonly int PositionOffset = 0;
	}
}
