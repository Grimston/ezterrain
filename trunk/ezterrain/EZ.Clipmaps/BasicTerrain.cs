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
		private TerrainGrid grid;

		public BasicTerrain(int sideVertexCount)
		{
			grid = new TerrainGrid(sideVertexCount);
		}

		public RenderGroup RenderGroup
		{
			get { return RenderGroup.Opaque; }
		}

		public bool Initialized { get; private set; }

		public void Initialize()
		{
			grid.InitializeBuffers();
			Initialized = true;
		}

		public bool Update(RenderInfo info)
		{
			return true;
		}

		public void Render(RenderInfo info)
		{
			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

			using (grid.Use())
			{
				grid.Draw(true);
			}
		}
	}
}
