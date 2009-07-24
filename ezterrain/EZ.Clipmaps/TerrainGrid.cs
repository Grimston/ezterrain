using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Objects;
using OpenTK.Graphics;
using EZ.Core;

namespace Ez.Clipmaps
{
	class TerrainGrid : Disposable, IBound
	{
		private VertexBufferObject<VertexP> vertices;
		private VertexBufferObject<uint> fullGrid;
		private VertexBufferObject<uint> hollowGrid;

		public TerrainGrid(int sideVertexCount)
		{
			this.SideVertexCount = sideVertexCount;
			InitializeVertices();
			InitializeFullGrid();
			InitializeHollowGrid();
		}

		private void InitializeHollowGrid()
		{
			hollowGrid = new VertexBufferObject<uint>(EnableCap.IndexArray,
														BufferTarget.ElementArrayBuffer,
														BufferUsageHint.StaticDraw);
			hollowGrid.Buffer.AddRange(Grid.GetHollowGridIndexArray((uint)SideVertexCount));
		}

		private void InitializeFullGrid()
		{
			fullGrid = new VertexBufferObject<uint>(EnableCap.IndexArray,
													BufferTarget.ElementArrayBuffer,
													BufferUsageHint.StaticDraw);
			fullGrid.Buffer.AddRange(Grid.GetFullGridIndexArray((uint)SideVertexCount));
		}

		private void InitializeVertices()
		{
			vertices = new VertexBufferObject<VertexP>(EnableCap.VertexArray,
														BufferTarget.ArrayBuffer,
														BufferUsageHint.StaticDraw);
			vertices.Buffer.AddRange(Grid.GetCenteredVertexArray((uint)SideVertexCount));
		}

		public int SideVertexCount { get; private set; }

		public void InitializeBuffers()
		{
			InitializeBuffer(vertices, true);
			InitializeBuffer(fullGrid);
			InitializeBuffer(hollowGrid);
		}

		protected override void Dispose(bool nongc)
		{
			if (nongc)
			{
				vertices.Dispose();
				fullGrid.Dispose();
				hollowGrid.Dispose();
			}

			base.Dispose(nongc);
		}

		private void InitializeBuffer<T>(VertexBufferObject<T> buffer)
			where T : struct
		{
			InitializeBuffer(buffer, false);
		}

		private void InitializeBuffer<T>(VertexBufferObject<T> buffer, bool setPointer)
			where T : struct
		{
			buffer.Create();
			using (buffer.Use())
			{
				buffer.Upload();
				if (setPointer)
				{
					buffer.SetPointer();
				}
			}
		}

		public void Bind()
		{
			vertices.Bind();
			vertices.SetPointer();
		}

		public void Unbind()
		{
			vertices.Unbind();
		}

		public void DrawCenter()
		{
			using(fullGrid.Use())
			{
				GL.DrawElements(BeginMode.Triangles,
								fullGrid.Buffer.Count,
								DrawElementsType.UnsignedInt,
								IntPtr.Zero);
			}
		}

		public void DrawOuter(int numLevels)
		{	
			using(hollowGrid.Use())
			{
				GL.Ext.DrawElementsInstanced(BeginMode.Triangles,
				                         	 hollowGrid.Buffer.Count,
				                         	 DrawElementsType.UnsignedInt,
				                         	 IntPtr.Zero,
				                         	 numLevels);
			}
		}
	}
}
