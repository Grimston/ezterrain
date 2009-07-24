using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using EZ.Imaging;

namespace EZ.Texturing
{
	public class Texture2D : BoundTexture
	{
		private HashSet<Region2D> dirtyRegions;

		public Texture2D(TextureUnit unit, Image image)
			: base(unit, image)
		{
			dirtyRegions = new HashSet<Region2D>();
		}

		public override TextureTarget Target
		{
			get { return TextureTarget.Texture2D; }
		}

		protected override EnableCap? EnableCap
		{
			get { return OpenTK.Graphics.EnableCap.Texture2D; }
		}

		public override void Initialize()
		{
			if (!Initialized)
			{
				Dirty();

				base.Initialize();
			}
		}

		public void Dirty()
		{
			dirtyRegions.Clear();
			dirtyRegions.Add(Image.Bounds);
		}

		public void Dirty(Region2D region)
		{
			dirtyRegions.Add(region);
		}

		public override void Update()
		{
			base.Update();

			ITexImage texImage = Image as ITexImage;

			if (texImage != null)
			{
				foreach (Region2D region in dirtyRegions)
				{
					texImage.Upload(Target, region);
				}
			}

			dirtyRegions.Clear();
		}
	}
}
