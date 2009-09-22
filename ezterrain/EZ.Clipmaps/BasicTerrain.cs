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
		private TerrainTexture texture;
		private TerrainProgram program;

		public BasicTerrain(int sideVertexCount)
		{
			grid = new TerrainGrid(sideVertexCount);
			//texture = new TerrainTexture(TextureUnit.Texture0, sideVertexCount, "l{0}.bmp", MaxLevels);
			texture = new TerrainTexture(TextureUnit.Texture0, sideVertexCount, "island{0}.bt", MaxLevels);
			program = new TerrainProgram();
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

			program.Initialize();
			program.SetHeightMaps(texture.Unit);
			program.SetTexScale(1f/grid.SideVertexCount);

			Initialized = true;
		}

		public bool Update(RenderInfo info)
		{
			texture.SetEye(info.Viewer.Position);
			texture.Update();

			program.SetEyeOffset(info.Viewer.Position.Xy);

			return true;
		}

		private IEnumerable<IBound> BoundObjects
		{
			get
			{
				yield return texture;
				yield return program;
			}
		}
		
		public void Render(RenderInfo info)
		{
			using(BoundObjects.Use())
			{
				//GL.PushAttrib(AttribMask.PolygonBit);
				//GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);

				using (grid.Use())
				{
					program.IsCenter = true;
					grid.DrawCenter();

					program.IsCenter = false;
					grid.DrawOuter(MaxLevels-1);
				}
				
				//GL.PopAttrib();
			}
		}
	}
}
