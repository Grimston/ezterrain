using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics;
using System.Runtime.InteropServices;

namespace EZ.Objects
{
	public static class ImageHelper
	{
		public static IImage Get2DImage(string fileName)
		{
			Bitmap bitmap = (Bitmap)Bitmap.FromFile(fileName);
			switch (bitmap.PixelFormat)
			{
				case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
					return GetImage2D<BGR>(bitmap);
				case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
					return GetImage2D<BGRA>(bitmap);
				default:
					throw new NotImplementedException();
			}
		}

		private static Image2D<TPixel> GetImage2D<TPixel>(Bitmap bitmap)
			where TPixel : struct, IPixel
		{
			Image2D<TPixel> image = new Image2D<TPixel>(TextureTarget.Texture2D, bitmap.Width, bitmap.Height);

			byte[] buffer;
			int height;
			int stride;
			GetBytes(bitmap, out buffer, out height, out stride);

			for (int row = 0, dataIndex = 0; row < height; row++, dataIndex += stride)
			{
				for (int column = 0; column < bitmap.Width; column++)
				{
					TPixel pixel = new TPixel();
					pixel.CopyFrom(buffer, dataIndex + column * ImageData<TPixel>.PixelSize);
					image.Data.Buffer[column, row, 0] = pixel;
				}
			}

			return image;
		}

		public static IImage GetArrayImage(string fileName, int index)
		{
			Bitmap bitmap = (Bitmap)Bitmap.FromFile(fileName);
			switch (bitmap.PixelFormat)
			{
				case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
					return GetArrayImage<BGR>(bitmap, index);
				case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
					return GetArrayImage<BGRA>(bitmap, index);
				default:
					throw new NotImplementedException();
			}
		}

		private static IImage GetArrayImage<TPixel>(Bitmap bitmap, int index)
			where TPixel : struct, IPixel
		{
			Array2DImage<TPixel> image = new Array2DImage<TPixel>(index, bitmap.Width, bitmap.Height);

			byte[] buffer;
			int height;
			int stride;
			GetBytes(bitmap, out buffer, out height, out stride);

			for (int row = 0, dataIndex = 0; row < height; row++, dataIndex += stride)
			{
				for (int column = 0; column < bitmap.Width; column++)
				{
					TPixel pixel = new TPixel();
					pixel.CopyFrom(buffer, dataIndex + column * ImageData<TPixel>.PixelSize);
					image.Data.Buffer[column, row, 0] = pixel;
				}
			}

			return image;
		}

		private static void GetBytes(Bitmap bitmap, out byte[] buffer, out int height, out int stride)
		{
			System.Drawing.Imaging.BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
																	 System.Drawing.Imaging.ImageLockMode.ReadOnly,
																	 bitmap.PixelFormat);
			height = data.Height;
			stride = data.Stride;
			buffer = new byte[data.Height * data.Stride];
			Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
			bitmap.UnlockBits(data);
		}

		public static int Width(this IImage image)
		{
			return image.Data.Size.Width;
		}

		public static int Height(this IImage image)
		{
			return image.Data.Size.Height;
		}

		public static int Depth(this IImage image)
		{
			return image.Data.Size.Depth;
		}
	}
}
