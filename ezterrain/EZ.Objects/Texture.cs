using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace EZ.Objects
{
	public abstract class Texture : Disposable
	{
		protected Texture(IImage image)
		{
			this.Image = image;
		}

		public IImage Image { get; private set; }

		public bool Initialized { get; protected set; }

		public abstract void Initialize();

		public virtual void Update()
		{
			Image.Update();
		}

		protected override void Dispose(bool nongc)
		{
			if (nongc)
			{
				//Image.Dispose();
			}

			base.Dispose(nongc);
		}
	}
}
