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
		public Texture(string fileName)
			: this(new Bitmap(fileName))
		{ }

		public Texture(Stream stream)
			: this(new Bitmap(stream))
		{ }

		public Texture(Bitmap bitmap)
		{
			this.Bitmap = bitmap;
			this.DirtyRegions = new List<Rectangle>();
		}

		public Bitmap Bitmap { get; private set; }

		public bool Initialized { get; protected set; }

		public abstract void Initialize();

		public Rectangle Bounds
		{
			get
			{
				return new Rectangle(0, 0, Bitmap.Width, Bitmap.Height);
			}
		}

		public ICollection<Rectangle> DirtyRegions { get; private set; }

		public virtual void Update()
		{
			ClearDirtyRegions();
		}

		private void ClearDirtyRegions()
		{
			foreach (Rectangle dirtyRegion in DirtyRegions)
			{
				Upload(dirtyRegion);
			}

			DirtyRegions.Clear();
		}


		private void Upload(Rectangle dirtyRegion)
		{
			BitmapData data = Bitmap.LockBits(dirtyRegion, ImageLockMode.ReadOnly, Bitmap.PixelFormat);

			Upload(data);

			Bitmap.UnlockBits(data);
		}

		protected abstract void Upload(BitmapData data);

		protected override void Dispose(bool nongc)
		{
			if (nongc)
			{
				Bitmap.Dispose();
			}

			base.Dispose(nongc);
		}
	}
}
