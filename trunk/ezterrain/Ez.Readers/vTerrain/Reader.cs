using System;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Ez.Readers.vTerrain
{
	public class Reader : IDisposable
	{
		private BinaryReader reader;
		private IDataReader dataReader;
		
		public Reader (string fileName)
			: this(File.OpenRead(fileName))
		{ }
		
		public Reader(Stream stream)
		{
			this.reader = new BinaryReader(stream, Encoding.ASCII);
			
			CheckVersion();
			this.Header = ReadHeader();
		}
		
		~Reader()
		{
			Dispose(false);
		}
		
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		protected virtual void Dispose(bool nongc)
		{
			if(nongc)
			{
				Close();
			}
		}
		
		public void Close()
		{
			reader.Close();
		}
		
		public Header Header { get; private set; }
		
		private void CheckVersion()
		{
			reader.BaseStream.Seek(0, SeekOrigin.Begin);
			bool valid = reader.ReadChar() == 'b';
			valid &= reader.ReadChar() == 'i';
			valid &= reader.ReadChar() == 'n';
			valid &= reader.ReadChar() == 't';
			valid &= reader.ReadChar() == 'e';
			valid &= reader.ReadChar() == 'r';
			valid &= reader.ReadChar() == 'r';
			
			if(!valid)
			{
				throw new InvalidDataException("Invalid BT file");
			}
		}
		
		private Header ReadHeader()
		{
			Header header = new Header();
			reader.BaseStream.Seek(10, SeekOrigin.Begin);
			header.Columns = reader.ReadInt32();
			header.Rows = reader.ReadInt32();
			header.DataSize = (DataSize)reader.ReadInt16();
			header.DataType = (DataType)reader.ReadInt16();
			switch (header.DataSize)
			{
			case DataSize.TwoBytes:
				throw new InvalidEnumArgumentException();
			case DataSize.FourBytes:
				switch (header.DataType)
				{
				case DataType.Float:
					this.dataReader = new FloatReader(reader, 256, header.Columns, header.Rows);
					break;
				default:
					throw new InvalidEnumArgumentException();
				}
				break;
			default:
				throw new InvalidEnumArgumentException();
			}
			
			header.Projection = (ProjectionType)reader.ReadInt16();
			header.UtmZone = reader.ReadInt16();
			header.Datum = (Datums)reader.ReadInt16();
			header.Left = reader.ReadDouble();
			header.Right = reader.ReadDouble();
			header.Bottom = reader.ReadDouble();
			header.Top = reader.ReadDouble();
			header.ExternalProjection = reader.ReadInt16();
			header.VerticalScale = reader.ReadSingle();

			if(header.VerticalScale == 0)
			{
				header.VerticalScale = 1;
			}
			
			return header;
		}
		
		public void Read (int column, int row, int width, int height, IntPtr buffer, int skipRows, int skipColumns, int bufferWidth, int bufferHeight)
		{
			dataReader.Read(column,row, width, height, buffer, skipRows, skipColumns, bufferWidth, bufferHeight);
		}
	}
}
