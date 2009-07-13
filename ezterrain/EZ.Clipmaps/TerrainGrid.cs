using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Objects;
using OpenTK.Graphics;

namespace Ez.Clipmaps
{
	public class TerrainGrid : Disposable
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
			InitializeBuffer(vertices);
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
			buffer.Create();
			buffer.Bind();
			buffer.Upload();
			buffer.Unbind();
		}

		public void Bind()
		{
			vertices.Bind();
		}

		public void Draw(int level)
		{
			if (level == 0)
			{
				Draw(fullGrid);
			}
			else
			{
				Draw(hollowGrid);
			}
		}

		private void Draw(VertexBufferObject<uint> elements)
		{
			elements.Bind();
			GL.DrawElements(BeginMode.Triangles,
							elements.Buffer.Count,
							DrawElementsType.UnsignedInt,
							IntPtr.Zero);
			elements.Unbind();
		}
	}
}
