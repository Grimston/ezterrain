using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OpenTK.Graphics;

namespace EZ.Objects
{
	public abstract class ImageData
	{
		protected ImageData(int pixelSize, int width, int height, int depth)
		{
			Data = new byte[pixelSize * width * height * depth];
			this.PixelSize = pixelSize;
			this.Width = width;
			this.Height = height;
			this.Depth = depth;
		}

		public byte[] Data { get; private set; }

		public int PixelSize { get; private set; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		public int Depth { get; private set; }
	}

	public abstract class ImageData<TPixel> : ImageData
		where TPixel : struct, IPixel
	{
		public static readonly new int PixelSize = Marshal.SizeOf(typeof(TPixel));
		public static readonly PixelFormat PixelFormat = GetAttribute<PixelFormatAttribute>(typeof(TPixel)).PixelFormat;
		public static readonly PixelInternalFormat PixelInternalFormat = GetAttribute<PixelInternalFormatAttribute>(typeof(TPixel)).PixelInternalFormat;
		public static readonly PixelType PixelType = GetAttribute<PixelTypeAttribute>(typeof(TPixel)).PixelType;

		private static TAttribute GetAttribute<TAttribute>(Type type)
		{
			Type attributeType = typeof(TAttribute);
			foreach (TAttribute attribute
						in type.GetCustomAttributes(attributeType, false))
			{
				return attribute;
			}

			throw new ArgumentException("Given type should have " + attributeType, "type:" + type);
		}

		protected ImageData(int width, int height, int depth)
			: base(PixelSize, width, height, depth)
		{ }

		protected TPixel GetPixel(int dataIndex)
		{
			TPixel pixel = new TPixel();
			pixel.CopyFrom(Data, dataIndex);

			return pixel;
		}

		protected void SetPixel(int dataIndex, TPixel value)
		{
			value.CopyTo(Data, dataIndex);
		}
	}
}
