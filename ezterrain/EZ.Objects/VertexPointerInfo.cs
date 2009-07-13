using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;

namespace EZ.Objects
{
	public struct VertexPointerInfo
	{
		public VertexPointerInfo(int coordinateCount,
								 VertexPointerType pointerType, 
								 int stride,
								 int offset)
		{
			this.coordinateCount = coordinateCount;
			this.pointerType = pointerType;
			this.stride = stride;
			this.offset = offset;
		}

		private int coordinateCount;
		public int CoordinateCount
		{
			get { return coordinateCount; }
		}

		private VertexPointerType pointerType;
		public VertexPointerType PointerType
		{
			get { return pointerType; }
		}

		private int stride;
		public int Stride
		{
			get { return stride; }
		}

		private int offset;
		public int Offset
		{
			get { return offset; }
		}
	}
}
