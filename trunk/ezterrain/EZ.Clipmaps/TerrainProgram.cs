using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Objects;
using OpenTK.Graphics;
using EZ.Core;

namespace Ez.Clipmaps
{
	public class TerrainProgram : Program
	{
		private static readonly string VertexFile = ResourceManager.GetProgramPath("terrain.vert");
		private static readonly string FragmentFile = ResourceManager.GetProgramPath("terrain.frag");

		private Uniform noise;
		private Uniform texScale;
		private Uniform outer;

		public TerrainProgram()
			: base()
		{
			noise = new Uniform(this, "noise");
			texScale = new Uniform(this, "texScale");
			outer = new Uniform(this, "outer");
		}

		public void Initialize()
		{
			Shader vertexShader = Shader.FromFile(ShaderType.VertexShader, VertexFile);
			Shader fragmentShader = Shader.FromFile(ShaderType.FragmentShader, FragmentFile);
			
			base.Initialize(vertexShader, fragmentShader);							
		}

		public void SetNoise(int unit)
		{
			noise.SetValue(unit);
		}

		public void SetTexScale(float scale)
		{
			texScale.SetValue(scale);
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
