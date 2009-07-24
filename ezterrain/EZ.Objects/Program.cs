using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using EZ.Core;

namespace EZ.Objects
{
	public class Program : Disposable, IBound
	{
		public bool Initialized { get; private set; }

		public int Handle { get; private set; }

		private Shader vertexShader;
		private Shader fragmentShader;

		public void Bind()
		{
			GL.UseProgram(Handle);
		}

		public void Unbind()
		{
			GL.UseProgram(0);
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

				int status;
				GL.GetProgram(Handle, ProgramParameter.LinkStatus, out status);

				string info;
				GL.GetProgramInfoLog(Handle, out info);

				string message = string.IsNullOrEmpty(info) ? "Program linked succesfully"
															: string.Format("Program link result:{0}{1}",
																			Environment.NewLine, info);

				System.Diagnostics.Debug.WriteLine(message);

				if (status == 0)
				{
					throw new GraphicsException(message);
				}

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
