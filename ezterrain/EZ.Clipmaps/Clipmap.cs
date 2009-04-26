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
		private uint sideVertexCount;
		uint[] indices;
		private Texture texture;
		private Program program;

		int vertexBuffer;
		int indexBuffer;

		public Clipmap()
		{
			sideVertexCount = 17;
			texture = new Texture(ResourceManager.GetImagePath("noise.bmp"));
			program = new Program();
		}

		public bool Initialized { get; private set; }

		public void Initialize()
		{
			texture.Initialize();

			program.Initialize(Shader.FromFile(ShaderType.VertexShader, ResourceManager.GetProgramPath("clipmap.vert")),
							   Shader.FromFile(ShaderType.FragmentShader, ResourceManager.GetProgramPath("clipmap.frag")));

			new Uniform(program, "noise").SetValue(0);
			new Uniform(program, "texScale").SetValue(1.0f / (sideVertexCount - 1));
			new Uniform(program, "texOffset").SetValue(0.0f);

			VertexP[] vertices = Grid.GetVertexArray(sideVertexCount);

			indices = Grid.GetIndexArray(sideVertexCount, 0);

			GL.GenBuffers(1, out vertexBuffer);
			GL.GenBuffers(1, out indexBuffer);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer,
						  (IntPtr)(vertices.Length * VertexP.SizeInBytes),
						  vertices,
						  BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
			GL.BufferData(BufferTarget.ElementArrayBuffer,
						  new IntPtr(indices.Length * 4/*size of an index*/),
						  indices,
						  BufferUsageHint.StaticDraw);

			//construct meshes
			Initialized = true;
		}

		public bool Update(RenderInfo info)
		{
			//TODO: update texture coordinates

			//always render
			return true;
		}

		public void Render(RenderInfo info)
		{
			texture.Bind();
			program.Bind();

			GL.EnableClientState(EnableCap.VertexArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);

			GL.VertexPointer(3, VertexPointerType.Float,
							 0,
							 (IntPtr)VertexP.PositionOffset);

			GL.DrawElements(BeginMode.Triangles,
							indices.Length,
							DrawElementsType.UnsignedInt,
							IntPtr.Zero);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			GL.DisableClientState(EnableCap.VertexArray);

			program.Unbind();
			texture.Unbind();
		}
	}
}
