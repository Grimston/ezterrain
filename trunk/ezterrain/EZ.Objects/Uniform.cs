using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;

namespace EZ.Objects
{
	public class Uniform
	{
		public Uniform(Program program, string name)
		{
			Location = GL.GetUniformLocation(program.Handle, name);
		}

		public int Location { get; private set; }

		public void SetValue(int value)
		{
			GL.Uniform1(Location, value);
		}

		public void SetValue(float value)
		{
			GL.Uniform1(Location, value);
		}
	}
}
