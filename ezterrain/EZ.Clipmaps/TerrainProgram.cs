using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Objects;
using OpenTK.Graphics;
using EZ.Core;
using OpenTK.Math;

namespace Ez.Clipmaps
{
	class TerrainProgram : Program
	{
		private static readonly string VertexFile = ResourceManager.GetProgramPath("terrain.vert");
		private static readonly string FragmentFile = ResourceManager.GetProgramPath("terrain.frag");

		private Uniform heightMaps;
		private Uniform texScale;
		private Uniform outer;
		private Uniform eyeOffset;

		public TerrainProgram()
			: base()
		{
			heightMaps = new Uniform(this, "heightMaps");
			texScale = new Uniform(this, "texScale");
			outer = new Uniform(this, "outer");
			eyeOffset = new Uniform(this, "eyeOffset");
		}

		public void Initialize()
		{
			Shader vertexShader = Shader.FromFile(ShaderType.VertexShader, VertexFile);
			Shader fragmentShader = Shader.FromFile(ShaderType.FragmentShader, FragmentFile);
			
			base.Initialize(vertexShader, fragmentShader);							
		}

		public void SetHeightMaps(TextureUnit unit)
		{
			heightMaps.SetValue(unit - TextureUnit.Texture0);
		}

		public void SetTexScale(float scale)
		{
			texScale.SetValue(scale);
		}

		public void SetEyeOffset(Vector2 offset)
		{
			eyeOffset.SetValue(offset);
		}

		public bool IsCenter
		{
			set
			{
				outer.SetValue(value ? 0f : 1f);
			}
		}
	}
}
