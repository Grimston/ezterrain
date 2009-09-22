using System;
using System.IO;

namespace Ez.Readers.vTerrain
{
	internal interface IDataReader
	{
		void Read(int column, int row, int width, int height, IntPtr buffer, int skipRows, int skipColumns, int bufferWidth, int bufferHeight);
	}
}
