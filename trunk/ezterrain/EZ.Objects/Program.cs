using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;

namespace EZ.Objects
{
	public class Program : Disposable
	{
		public bool Initialized { get; private set; }

		public int Handle { get; private set; }

		private Shader vertexShader;
		private Shader fragmentShader;

		public void Bind()
		{
			GL.UseProgram(Handle);
		}

		public void Initialize(Shader vertexShader, Shader fragmentShader)
		{
			if (!Initialized)
			{
				this.vertexShader = vertexShader;
				this.fragmentShader = fragmentShader;

				if (!vertexShader.Initialized)
				{
					vertexShader.Initialize();
				}

				if (!fragmentShader.Initialized)
				{
					fragmentShader.Initialize();
				}

				Handle = GL.CreateProgram();

				GL.AttachShader(Handle, vertexShader.Handle);
				GL.AttachShader(Handle, fragmentShader.Handle);

				GL.LinkProgram(Handle);

				Initialized = true;
			}
		}

		protected override void Dispose(bool nongc)
		{
			if (nongc && Initialized)
			{
					GL.DetachShader(Handle, vertexShader.Handle);
					GL.DetachShader(Handle, fragmentShader.Handle);

					vertexShader.Dispose();
					fragmentShader.Dispose();

					vertexShader = null;
					fragmentShader = null;

					GL.DeleteProgram(Handle);

					Handle = 0;
					Initialized = false;
			}
		}
	}
}
