using System;
using System.IO;

namespace Ez.Readers.vTerrain
{
	internal class FloatReader : IDataReader
	{
		private unsafe struct BufferIndex
		{
			public float* Buffer;
			public int Row;
			public int Column;
			public int Height;
			public int Width;
		}
		
		private BinaryReader reader;
		private int startOffset;
		private int width;
		private int height;
			
		public FloatReader (BinaryReader reader, int startOffset, int width, int height)
		{
			this.reader = reader;
			this.startOffset = startOffset;
			this.width = width;
			this.height = height;
		}
		
		private unsafe void Read(int column, int row, int width, int height, float* buffer, int skipRows, int skipColumns, int bufferWidth, int bufferHeight)
		{
				BufferIndex index = new BufferIndex();
				index.Buffer = buffer;
				index.Row = skipRows;
				index.Height = bufferHeight;
				index.Width = bufferWidth;

				int targetColumn = column + width;
				int targetRow = row + height;
				
				int currentColumn = column;
				index.Column = skipColumns;

				//currentColumn < 0
				for (; currentColumn < 0 && currentColumn < targetColumn; 
				     ++currentColumn, ++index.Column)
				{
					index.Row = skipRows;
	
					for (int currentRow = row; 
					     currentRow < targetRow;
					     ++currentRow, ++index.Row)
					{
						SetValue(ref index, 0);
					}
				}
				
				//currentcolumn in range
				for (; currentColumn < this.width && currentColumn < targetColumn; 
				     ++currentColumn, ++index.Column)
				{
					index.Row = skipRows;
					int currentRow = row;
					
					//currentRow < 0
					for (; currentRow < 0 && currentRow < targetRow;
					     ++currentRow, ++index.Row)
					{
						SetValue(ref index, 0);
					}
					
					//currentRow in range
					if(currentRow < targetRow)//check whether there is need to seek
					{
						Seek(currentColumn, currentRow);
						for (; currentRow < this.height && currentRow < targetRow;
						     ++currentRow, ++index.Row)
						{
							SetValue(ref index, reader.ReadSingle());
						}
						
						//currentRow >= height
						for (; currentRow < targetRow;
						     ++currentRow, ++index.Row)
						{
							SetValue(ref index, 0);
						}
					}
				}
				
				//currentColumn >= width
				for (; currentColumn < targetColumn; 
				     ++currentColumn, ++index.Column)
				{
					index.Row = skipRows;
	
					for (int currentRow = row; 
					     currentRow < targetRow;
					     ++currentRow, ++index.Row)
					{
						SetValue(ref index, 0);
					}
				}
		}
		
		private void Seek(int column, int row)
		{
			int position = this.height * column + row;
			reader.BaseStream.Seek(startOffset + position * sizeof(float), SeekOrigin.Begin);
		}
		
		private unsafe void SetValue(ref BufferIndex index, float value)
		{
			index.Buffer[(index.Row + index.Height) % index.Height * index.Width + (index.Column + index.Width) % index.Width] = value;
		}
		
		public unsafe void Read (int column, int row, int width, int height, IntPtr buffer, int skipRows, int skipColumns, int bufferWidth, int bufferHeight)
		{
			Read(column, row, width, height, (float*)buffer.ToPointer(), skipRows, skipColumns, bufferWidth, bufferHeight);
		}
	}
}
