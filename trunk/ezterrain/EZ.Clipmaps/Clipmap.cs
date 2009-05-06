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
		uint[] hollowGridIndices;
		uint[] fullGridIndices;
		private List<Pair<Texture, Uniform>> texuniPairs;
		private Texture gradient;
		private Program program;
		private Uniform texScale;
		private Uniform texOffset;
		private Uniform meshLevel;
		private Uniform eye;

		int vertexBuffer;
		int fullGridIndexBuffer;
		int hollowGridIndexBuffer;

		public Clipmap(uint sideVertexCount)
		{
			this.sideVertexCount = sideVertexCount;
			gradient = new Texture(TextureUnit.Texture0, ResourceManager.GetImagePath("gradient.bmp"));
			program = new Program();
			ConstructTexUniPairs();
			texScale = new Uniform(program, "texScale");
			texOffset = new Uniform(program, "texOffset");
			meshLevel = new Uniform(program, "level");
			eye = new Uniform(program, "eye");
		}

		private void ConstructTexUniPairs()
		{
			texuniPairs = new List<Pair<Texture, Uniform>>();

			string name = "noise";

			for (int i = 0; i < 4; i++)
			{
				Texture texture = new Texture(TextureUnit.Texture1 + i, ResourceManager.GetImagePath("noise.bmp"));
				Uniform uniform = new Uniform(program, name + i);

				texuniPairs.Add(new Pair<Texture, Uniform>(texture, uniform));
			}
		}

		public bool Initialized { get; private set; }

		public void Initialize()
		{
			texuniPairs.ForEach(pair => pair.Value1.Initialize());
			gradient.Initialize();

			program.Initialize(Shader.FromFile(ShaderType.VertexShader, ResourceManager.GetProgramPath("clipmap.vert")),
							   Shader.FromFile(ShaderType.FragmentShader, ResourceManager.GetProgramPath("clipmap.frag")));

			new Uniform(program, "gradient").SetValue(0);
			new Uniform(program, "heightScale").SetValue((float)Math.Log(sideVertexCount, 1.2));
			Vector3 light = new Vector3(0, 0, 1);
			light.Normalize();
			new Uniform(program, "lightDirection").SetValue(light);
			texScale.SetValue(1.0f / (sideVertexCount - 1));
			texOffset.SetValue(0.0f);
			meshLevel.SetValue(0.0f);
			

			VertexP[] vertices = Grid.GetCenteredVertexArray(sideVertexCount);

			//indices = Grid.GetIndexArray(sideVertexCount, 0);
			this.hollowGridIndices = Grid.GetHollowGridIndexArray(sideVertexCount);
			this.fullGridIndices = Grid.GetFullGridIndexArray(sideVertexCount);

			GL.GenBuffers(1, out vertexBuffer);
			GL.GenBuffers(1, out fullGridIndexBuffer);
			GL.GenBuffers(1, out hollowGridIndexBuffer);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer,
						  (IntPtr)(vertices.Length * VertexP.SizeInBytes),
						  vertices,
						  BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, fullGridIndexBuffer);
			GL.BufferData(BufferTarget.ElementArrayBuffer,
						  new IntPtr(fullGridIndices.Length * 4/*size of an index*/),
						  fullGridIndices,
						  BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, hollowGridIndexBuffer);
			GL.BufferData(BufferTarget.ElementArrayBuffer,
						  new IntPtr(hollowGridIndices.Length * 4/*size of an index*/),
						  hollowGridIndices,
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
			gradient.Bind();
			program.Bind();

			texuniPairs.ForEach((pair) => { pair.Value1.Bind(); pair.Value2.SetValue(pair.Value1.UnitIndex); });

			eye.SetValue(info.Viewer.Position);

			int distanceScale = (int)(Math.Sqrt(Math.Abs(info.Viewer.Position.Z)) / 16);

			GL.EnableClientState(EnableCap.VertexArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
			GL.VertexPointer(3, VertexPointerType.Float,
							 0,
							 (IntPtr)VertexP.PositionOffset);

			texScale.SetValue(1.0f / (sideVertexCount - 1));
			texOffset.SetValue(0.5f);

			meshLevel.SetValue(0.0f + distanceScale);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, fullGridIndexBuffer);
			GL.DrawElements(BeginMode.Triangles,
							fullGridIndices.Length,
							DrawElementsType.UnsignedInt,
							IntPtr.Zero);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, hollowGridIndexBuffer);
			for (float i = 1.0f; i < 3.0f; i++)
			{
				meshLevel.SetValue(i + distanceScale);
				GL.DrawElements(BeginMode.Triangles,
								hollowGridIndices.Length,
								DrawElementsType.UnsignedInt,
								IntPtr.Zero);
			}

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			GL.DisableClientState(EnableCap.VertexArray);

			program.Unbind();
			gradient.Unbind();
			texuniPairs.ForEach((pair) => pair.Value1.Unbind());
		}
	}
}
