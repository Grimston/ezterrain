using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenTK.Graphics;

namespace EZ.Objects
{
	public class Shader : Disposable
	{
		public Shader(ShaderType type, string source)
		{
			this.Type = type;
			this.Source = source;
		}

		public ShaderType Type { get; private set; }

		public string Source { get; private set; }

		public void LoadSource(string fileName)
		{
			using (StreamReader reader = new StreamReader(fileName))
			{
				Source = reader.ReadToEnd();
			}
		}

		public static Shader FromFile(ShaderType type, string fileName)
		{
			Shader shader = new Shader(type, string.Empty);
			shader.LoadSource(fileName);

			return shader;
		}

		public bool Initialized { get; private set; }

		public int Handle { get; private set; }

		public void Initialize()
		{
			if (!Initialized)
			{
				Handle = GL.CreateShader(Type);

				GL.ShaderSource(Handle, Source);

				GL.CompileShader(Handle);

				int status;
				GL.GetShader(Handle, ShaderParameter.CompileStatus, out status);

				if (status == 0)
				{
					string info;
					GL.GetShaderInfoLog(Handle, out info);

					string message = string.Format("{0} cannot be compiled:{1}{2}",
													Type, Environment.NewLine, info);

					System.Diagnostics.Debug.WriteLine(message);

					throw new GraphicsException(message);
				}

				Initialized = true;
			}
		}

		protected override void Dispose(bool nongc)
		{
			if (nongc && Initialized)
			{
				GL.DeleteShader(Handle);

				Initialized = false;
				Handle = 0;
			}
		}
	}
}
