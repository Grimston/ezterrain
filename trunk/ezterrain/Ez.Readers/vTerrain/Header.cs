using System;
using System.Runtime.InteropServices;

namespace Ez.Readers.vTerrain
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Header
	{
		public int Columns;
		public int Rows;
		public DataSize DataSize;
		public DataType DataType;
		public ProjectionType Projection;
		public short UtmZone;
		public Datums Datum;
		public double Left, Right, Bottom, Top;
		public short ExternalProjection;
		public float VerticalScale;
		
		public override string ToString ()
		{
			return string.Format("{0}x{1}, size: {2} of type {3}\n"
			                     + "{4} in zone: {5} with datum: {6}\n"
			                     + "l: {7} r:{8} b:{9} t:{10}\n"
			                     + "proj: {11}, scale:{12}\n\n",
			                     Columns, Rows, DataSize, DataType,
			                     Projection, UtmZone, Datum,
			                     Left, Right, Bottom, Top,
			                     ExternalProjection, VerticalScale);
		}

	}
}
