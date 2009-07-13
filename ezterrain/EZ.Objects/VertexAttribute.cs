using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;

namespace EZ.Objects
{
	[global::System.AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	public sealed class VertexAttribute : Attribute
	{
		public VertexAttribute(int coordinateCount, VertexPointerType pointerType)
			: this(coordinateCount, pointerType, 0, 0)
		{ }

		public VertexAttribute(int coordinateCount, VertexPointerType pointerType, int stride, int offset)
		{
			this.CoordinateCount = coordinateCount;
			this.PointerType = pointerType;
			this.Stride = stride;
			this.Offset = offset;
		}

		public int CoordinateCount { get; private set; }

		public VertexPointerType PointerType { get; private set; }

		public int Stride { get; private set; }

		public int Offset { get; private set; }
	}
}
