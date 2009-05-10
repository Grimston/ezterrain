using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using System.Drawing;
using System.IO;

namespace EZ.Objects
{
	public abstract class BoundTexture : Texture
	{
		public BoundTexture(TextureUnit unit, string fileName)
			: base(fileName)
		{
			this.Unit = unit;
		}

		public BoundTexture(TextureUnit unit, Stream stream)
			: base(stream)
		{
			this.Unit = unit;
		}

		public BoundTexture(TextureUnit unit, Bitmap bitmap)
			: base(bitmap)
		{
			this.Unit = unit;
		}

		public TextureUnit Unit { get; set; }

		public int UnitIndex
		{
			get { return (int)this.Unit - (int)TextureUnit.Texture0; }
		}

		public int Handle { get; private set; }

		public override void Initialize()
		{
			if (!Initialized)
			{
				Handle = GL.GenTexture();

				DirtyRegions.Add(Bounds);

				Initialized = true;
			}
		}


		public override void Update()
		{
			Bind();

			base.Update();
		}

		public void Bind()
		{
			GL.Enable(EnableCap.Texture2D);
			GL.ActiveTexture(Unit);
			GL.BindTexture(Target, Handle);
		}

		public void Unbind()
		{
			GL.ActiveTexture(Unit);
			GL.BindTexture(Target, 0);
			GL.Disable(EnableCap.Texture2D);
		}

		protected override void Dispose(bool nongc)
		{
			if (nongc && Initialized)
			{
				GL.DeleteTexture(Handle);

				Initialized = false;
				Handle = 0;
			}

			base.Dispose(nongc);
		}
	}
}
