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
			Buffer = new byte[pixelSize * width * height * depth];
			this.PixelSize = pixelSize;
			this.Width = width;
			this.Height = height;
			this.Depth = depth;
		}

		public byte[] Buffer { get; private set; }

		public int PixelSize { get; private set; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		public int Depth { get; private set; }
	}

	public abstract class ImageData<TPixel> : ImageData
		where TPixel : struct, IPixel
	{
		public static readonly new int PixelSize = Marshal.SizeOf(typeof(TPixel));

		protected ImageData(int width, int height, int depth)
			: base(PixelSize, width, height, depth)
		{ }

		protected TPixel GetPixel(int dataIndex)
		{
			TPixel pixel = new TPixel();
			pixel.CopyFrom(Buffer, dataIndex);

			return pixel;
		}

		protected void SetPixel(int dataIndex, TPixel value)
		{
			value.CopyTo(Buffer, dataIndex);
		}

		public static void Copy(ImageData<TPixel> src, PixelIndex3D srcIndex,
								ImageData<TPixel> dst, PixelIndex3D dstIndex,
								Size3D copySize)
		{
			int srcRowStride = src.Width * PixelSize;
			int srcDepthStride = srcRowStride * src.Height;
			int dstRowStride = dst.Width * PixelSize;
			int dstDepthStride = dstRowStride * dst.Height;

			for (int depth = 0; depth < copySize.Depth; depth++)
			{
				int srcDepth = (srcIndex.Depth + depth) * srcDepthStride;
				int dstDepth = (dstIndex.Depth + depth) * dstDepthStride;

				for (int row = 0; row < copySize.Height; row++)
				{
					int srcOffset = srcDepth + (srcIndex.Row + row) * srcRowStride 
									+ srcIndex.Column * PixelSize;
					int dstOffset = dstDepth + (dstIndex.Row + row) * dstRowStride 
									+ dstIndex.Column * PixelSize;

					System.Buffer.BlockCopy(src.Buffer, srcOffset,
											dst.Buffer, dstOffset, 
											copySize.Width * PixelSize);
				}
			}
		}
	}
}
