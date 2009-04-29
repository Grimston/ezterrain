using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Math;

namespace EZ.Objects
{
	public class Uniform
	{
		public Uniform(Program program, string name)
		{
			this.Program = program;
			this.Name = name;
		}

		public string Name { get; private set; }

		public Program Program { get; private set; }

		private int GetLocation()
		{
			Program.Bind();
			return GL.GetUniformLocation(Program.Handle, Name);
		}

		public void SetValue(int value)
		{
			GL.Uniform1(GetLocation(), value);
		}

		public void SetValue(float value)
		{
			GL.Uniform1(GetLocation(), value);
		}

		public void SetValue(Vector3 value)
		{
			GL.Uniform3(GetLocation(), value.X, value.Y, value.Z);
		}
	}
}
