using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Core;
using OpenTK.Math;
using OpenTK.Graphics;
using EZ.Objects;
using System.Runtime.InteropServices;

namespace Ez.Clipmaps
{
	public class Clipmap : IRenderable
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct Vertex
		{
			public Vertex(Vector3 position)
			{
				this.Position = position;
				this.TexCoord = position.Xy;
			}

			public Vertex(Vector3 position, Vector2 texCoord)
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

			public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Vertex));
			public static readonly int PositionOffset = 0;
			public static readonly int TexCoordOffset = PositionOffset + Vector3.SizeInBytes;
		}

		private List<Vertex> vertices;
		private List<uint> indices;
		private uint sideVertexCount;
		private Texture texture;

		int vertexBuffer;
		int indexBuffer;

		public Clipmap()
		{
			vertices = new List<Vertex>();
			indices = new List<uint>();
			sideVertexCount = 10;
			texture = new Texture("noise.bmp");
		}

		public bool Initialized { get; private set; }

		public void Initialize()
		{
			texture.Initialize();

			vertices.Clear();
			indices.Clear();

			float textureScale = 1.0f / (sideVertexCount - 1);

			for (int i = 0; i < sideVertexCount; i++)
			{
				for (int j = 0; j < sideVertexCount; j++)
				{
					Vertex vertex = new Vertex(new Vector3(i, j, 0), new Vector2(i, j) * textureScale);
					vertices.Add(vertex);
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
						  (IntPtr)(vertices.Count * 20/*size of a vertex*/),
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
			texture.Bind();

			GL.EnableClientState(EnableCap.VertexArray);
			GL.EnableClientState(EnableCap.TextureCoordArray);
			//draw vertex buffers
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);

			GL.TexCoordPointer(2, TexCoordPointerType.Float, 
							   Vertex.SizeInBytes,
							   (IntPtr)Vertex.TexCoordOffset);

			GL.VertexPointer(3, VertexPointerType.Float,
							 Vertex.SizeInBytes,
							 (IntPtr)Vertex.PositionOffset);

			GL.DrawElements(BeginMode.Triangles,
							indices.Count,
							DrawElementsType.UnsignedInt,
							IntPtr.Zero);
		}
	}
}
