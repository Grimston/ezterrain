using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using EZ.Core;
using EZ.Imaging;

namespace EZ.Texturing
{
	public abstract class Texture : Disposable
	{
		protected Texture(Image image)
		{
			this.Image = image;
		}

		public Image Image { get; private set; }

		public bool Initialized { get; protected set; }

		public abstract void Initialize();

		public abstract void Update();
	}
}
