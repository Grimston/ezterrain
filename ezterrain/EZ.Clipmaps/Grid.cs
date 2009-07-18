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

		public static VertexP[] GetCenteredVertexArray(uint sideVertexCount)
		{
			VertexP[] vertices = new VertexP[sideVertexCount * sideVertexCount];
			float offset = sideVertexCount / 2;

			for (int i = 0, index = 0; i < sideVertexCount; i++)
			{
				for (int j = 0; j < sideVertexCount; j++, index++)
				{
					vertices[index] = new VertexP(i - offset, j - offset, 0);
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
			uint[] indices = new uint[GetIndexCount(sideVertexCount, level)];

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


		#region Full Grid
		public static void GenerateFullGrid(List<uint> indices,
											uint rowIndex, uint columnIndex,
											uint rowEndIndex, uint columnEndIndex,
											uint sideVertexCount)
		{
			for (uint i = rowIndex; i < rowEndIndex; i++)
			{
				for (uint j = columnIndex; j < columnEndIndex; j++)
				{
					indices.Add(i * sideVertexCount + j);
					indices.Add((i + 1) * sideVertexCount + j);
					indices.Add(i * sideVertexCount + j + 1);

					indices.Add((i + 1) * sideVertexCount + j);
					indices.Add((i + 1) * sideVertexCount + j + 1);
					indices.Add(i * sideVertexCount + j + 1);
				}
			}
		}
		#endregion

		#region Hollow Grid

		private struct GridParameters
		{
			public GridParameters(uint rowIndex, uint columnIndex,
								  uint rowEndIndex, uint columnEndIndex)
			{
				this.RowIndex = rowIndex;
				this.RowEndIndex = rowEndIndex;
				this.ColumnIndex = columnIndex;
				this.ColumnEndIndex = columnEndIndex;
			}

			public uint RowIndex;
			public uint RowEndIndex;
			public uint ColumnIndex;
			public uint ColumnEndIndex;
		}

		public static void GenerateHollowGrid(List<uint> indices,
											  uint rowIndex, uint columnIndex,
											  uint rowEndIndex, uint columnEndIndex,
											  uint sideVertexCount)
		{
			uint rowCount = rowEndIndex - rowIndex;
			uint columnCount = columnEndIndex - columnIndex;

			GridParameters[] parameters = new GridParameters[4]
			{
				new GridParameters(rowIndex, columnIndex, rowIndex + rowCount / 4, columnEndIndex),
				new GridParameters(rowIndex + rowCount / 4, columnIndex, rowEndIndex - rowCount / 4, columnCount / 4 + 1),
				new GridParameters(rowIndex + rowCount / 4, columnEndIndex - columnCount / 4, rowEndIndex - rowCount / 4, columnEndIndex),
				new GridParameters(rowEndIndex - rowCount / 4, columnIndex, rowEndIndex, columnEndIndex)
			};

			for (int i = 0; i < parameters.Length; i++)
			{
				GenerateFullGrid(indices,
								 parameters[i].RowIndex, parameters[i].ColumnIndex,
								 parameters[i].RowEndIndex, parameters[i].ColumnEndIndex,
								 sideVertexCount);
			}
		}
		#endregion

		#region Upper Connector
		public static void GenerateUpperConnector(List<uint> indices, uint sideVertexCount)
		{
			uint i;
			for (i = 0; i < sideVertexCount - 3; i += 2)
			{
				indices.Add(i);
				indices.Add(i + sideVertexCount + 1);
				indices.Add(i + 2);

				indices.Add(i + sideVertexCount + 1);
				indices.Add(i + sideVertexCount + 2);
				indices.Add(i + 2);

				indices.Add(i + sideVertexCount + 2);
				indices.Add(i + sideVertexCount + 3);
				indices.Add(i + 2);
			}

			indices.Add(i);
			indices.Add(i + sideVertexCount + 1);
			indices.Add(i + 2);
		}
		#endregion

		#region Right Connector
		public static void GenerateRightConnector(List<uint> indices, uint sideVertexCount)
		{
			uint i;
			uint totalVertexCount = sideVertexCount * sideVertexCount;
			uint increment = 2 * sideVertexCount;

			for (i = sideVertexCount - 1; i < totalVertexCount - increment - 1; i += increment)
			{
				indices.Add(i);
				indices.Add(i + sideVertexCount - 1);
				indices.Add(i + increment);

				indices.Add(i + sideVertexCount - 1);
				indices.Add(i + increment - 1);
				indices.Add(i + increment);

				indices.Add(i + increment - 1);
				indices.Add(i + increment + sideVertexCount - 1);
				indices.Add(i + increment);
			}

			indices.Add(i);
			indices.Add(i + sideVertexCount - 1);
			indices.Add(i + increment);
		}
		#endregion

		#region Lower Connector
		public static void GenerateLowerConnector(List<uint> indices, uint sideVertexCount)
		{
			uint i;
			uint totalVertexCount = sideVertexCount * sideVertexCount;
			uint decrement = 2;

			for (i = totalVertexCount - 1; i > totalVertexCount - sideVertexCount + 2; i -= decrement)
			{
				indices.Add(i);
				indices.Add(i - sideVertexCount - 1);
				indices.Add(i - decrement);

				indices.Add(i - sideVertexCount - 1);
				indices.Add(i - decrement - sideVertexCount);
				indices.Add(i - decrement);

				indices.Add(i - decrement - sideVertexCount);
				indices.Add(i - decrement - sideVertexCount - 1);
				indices.Add(i - decrement);
			}

			indices.Add(i);
			indices.Add(i - sideVertexCount - 1);
			indices.Add(i - decrement);
		}
		#endregion

		#region Left Connector
		public static void GenerateLeftConnector(List<uint> indices, uint sideVertexCount)
		{
			uint i;
			uint totalVertexCount = sideVertexCount * sideVertexCount;
			uint decrement = 2 * sideVertexCount;

			for (i = totalVertexCount - sideVertexCount; i > decrement; i -= decrement)
			{
				indices.Add(i);
				indices.Add(i - sideVertexCount + 1);
				indices.Add(i - decrement);

				indices.Add(i - sideVertexCount + 1);
				indices.Add(i - decrement + 1);
				indices.Add(i - decrement);

				indices.Add(i - decrement + 1);
				indices.Add(i - decrement - sideVertexCount + 1);
				indices.Add(i - decrement);
			}

			indices.Add(i);
			indices.Add(i - sideVertexCount + 1);
			indices.Add(i - decrement);
		}
		#endregion

		public static uint[] GetFullGridIndexArray(uint sideVertexCount)
		{
			List<uint> indices = new List<uint>();
			GenerateFullGrid(indices, 1, 1, sideVertexCount - 2, sideVertexCount - 2, sideVertexCount);
			GenerateUpperConnector(indices, sideVertexCount);
			GenerateRightConnector(indices, sideVertexCount);
			GenerateLowerConnector(indices, sideVertexCount);
			GenerateLeftConnector(indices, sideVertexCount);

			return indices.ToArray();
		}

		public static uint[] GetHollowGridIndexArray(uint sideVertexCount)
		{
			List<uint> indices = new List<uint>();
			GenerateHollowGrid(indices, 1, 1, sideVertexCount - 2, sideVertexCount - 2, sideVertexCount);
			GenerateUpperConnector(indices, sideVertexCount);
			GenerateRightConnector(indices, sideVertexCount);
			GenerateLowerConnector(indices, sideVertexCount);
			GenerateLeftConnector(indices, sideVertexCount);

			return indices.ToArray();
		}
	}
}
