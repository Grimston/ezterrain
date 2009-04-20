using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Core;
using OpenTK.Math;
using OpenTK.Graphics;

namespace Ez.Clipmaps
{
	public class Clipmap : IRenderable
	{
		private List<Vector3> vertices;
		private List<uint> indices;
		private uint sideVertexCount;
		int vertexBuffer;
		int indexBuffer;

		public Clipmap()
		{
			vertices = new List<Vector3>();
			indices = new List<uint>();
			sideVertexCount = 10;
		}

		public bool Initialized { get; private set; }

		public void Initialize()
		{
			vertices.Clear();
			indices.Clear();

			for (int i = 0; i < sideVertexCount; i++)
			{
				for (int j = 0; j < sideVertexCount; j++)
				{
					vertices.Add(new Vector3(i, j, 0));
				}
			}

			for (uint i = 0; i < sideVertexCount - 1; i++)
			{
				for (uint j = 0; j < sideVertexCount - 1; j++)
				{
					indices.Add(i * sideVertexCount + j);
					indices.Add(i * sideVertexCount + j + 1);
					indices.Add((i + 1) * sideVertexCount + j);

					indices.Add((i + 1) * sideVertexCount + j);
					indices.Add(i * sideVertexCount + j + 1);
					indices.Add((i + 1) * sideVertexCount + j + 1);
				}
			}

			GL.GenBuffers(1, out vertexBuffer);
			GL.GenBuffers(1, out indexBuffer);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer,
						  (IntPtr)(vertices.Count * 12/*size of a vertex*/),
						  vertices.ToArray(),
						  BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
			GL.BufferData(BufferTarget.ElementArrayBuffer,
						  new IntPtr(indices.Count * 4/*size of an index*/),
						  indices.ToArray(),
						  BufferUsageHint.StaticDraw);

			//construct meshes
			Initialized = true;
		}

		public bool Update(RenderInfo info)
		{
			//update texture coordinates

			//always render
			return true;
		}

		public void Render(RenderInfo info)
		{
			//draw vertex buffers
			GL.BindBuffer(BufferTarget.ArrayBuffer,
						  vertexBuffer);

			GL.VertexPointer(3/*component count*/, 
							 VertexPointerType.Float, 
							 0, 0);

			GL.DrawElements(BeginMode.Triangles, 
							indices.Count, 
							DrawElementsType.UnsignedInt, 
							IntPtr.Zero);
		}
	}
}
