using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OpenTK.Math;

namespace Ez.Clipmaps
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPT
	{
		public VertexPT(Vector3 position)
		{
			this.Position = position;
			this.TexCoord = position.Xy;
		}

		public VertexPT(Vector3 position, Vector2 texCoord)
		{
			this.Position = position;
			this.TexCoord = texCoord;
		}

		public Vector3 Position;
		public Vector2 TexCoord;

		public override string ToString()
		{
			return string.Format("P{0}T{1}", Position, TexCoord);
		}

		public static readonly int SizeInBytes = Marshal.SizeOf(typeof(VertexPT));
		public static readonly int PositionOffset = 0;
		public static readonly int TexCoordOffset = PositionOffset + Vector3.SizeInBytes;
	}
}
