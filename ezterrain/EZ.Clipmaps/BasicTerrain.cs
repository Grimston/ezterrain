using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Core;
using OpenTK.Graphics;
using EZ.Objects;

namespace Ez.Clipmaps
{
	public class BasicTerrain : IRenderable
	{
		public const int MaxLevels = 4;

		private TerrainGrid grid;
		private Texture2D texture;
		
		public BasicTerrain(int sideVertexCount)
		{
			grid = new TerrainGrid(sideVertexCount);
			texture = new Texture2D(TextureUnit.Texture0, ImageHelper.Get2DImage(ResourceManager.GetImagePath("noise.bmp")));
		}

		public RenderGroup RenderGroup
		{
			get { return RenderGroup.Opaque; }
		}

		public bool Initialized { get; private set; }

		public void Initialize()
		{
			grid.InitializeBuffers();
			texture.Initialize();

			Initialized = true;
		}

		public bool Update(RenderInfo info)
		{
			return true;
		}

		private IEnumerable<IBound> BoundObjects
		{
			get
			{
				yield return texture;
			}
		}
		
		public void Render(RenderInfo info)
		{
			using(BoundObjects.Use())
			{
				GL.PushAttrib(AttribMask.PolygonBit);
				GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);

				using (grid.Use())
				{
					grid.DrawCenter();
					grid.DrawOuter(MaxLevels-2);
				}
				
				GL.PopAttrib();
			}
		}
	}
}
