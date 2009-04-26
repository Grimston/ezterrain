using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Clipmaps
{
	public static class Grid
	{
		public static VertexP[] GetVertexArray(uint sideVertexCount)
		{
			VertexP[] vertices = new VertexP[sideVertexCount * sideVertexCount];

			for (int i = 0, index = 0; i < sideVertexCount; i++)
			{
				for (int j = 0; j < sideVertexCount; j++, index++)
				{
					vertices[index] = new VertexP(i, j, 0);
				}
			}

			return vertices;
		}

		public static uint GetLevelStep(uint level)
		{
			return 1u << (int)level;
		}

		public static int GetIndexCount(uint sideVertexCount, uint level)
		{
			uint step = GetLevelStep(level);
			return (int)(6 * ((sideVertexCount - 1) * (sideVertexCount - 1) / (step * step)));
		}

		public static uint[] GetIndexArray(uint sideVertexCount, uint level)
		{
			uint step = GetLevelStep(level);
			uint[] indices = new uint[GetIndexCount(sideVertexCount,level)];

			for (uint i = 0, index = 0; i < sideVertexCount - 1; i += step)
			{
				for (uint j = 0; j < sideVertexCount - 1; j += step)
				{
					indices[index++] = i * sideVertexCount + j;
					indices[index++] = i * sideVertexCount + j + step;
					indices[index++] = (i + step) * sideVertexCount + j;

					indices[index++] = (i + step) * sideVertexCount + j;
					indices[index++] = i * sideVertexCount + j + step;
					indices[index++] = (i + step) * sideVertexCount + j + step;
				}
			}

			return indices;
		}
	}
}
