using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using System.Runtime.InteropServices;
using System.Drawing;

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
					return new Image2D<BGR>(TextureTarget.Texture2D, GetImageData<BGR>(bitmap));
				case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
					return new Image2D<BGRA>(TextureTarget.Texture2D, GetImageData<BGRA>(bitmap));
				default:
					throw new NotImplementedException();
			}
		}

		public static IImage GetArrayImage(string fileName, int index)
		{
			Bitmap bitmap = (Bitmap)Bitmap.FromFile(fileName);
			switch (bitmap.PixelFormat)
			{
				case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
					return new Array2DImage<BGR>(index, GetImageData<BGR>(bitmap));
				case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
					return new Array2DImage<BGRA>(index, GetImageData<BGRA>(bitmap));
				default:
					throw new NotImplementedException();
			}
		}

		private static ImageData<TPixel> GetImageData<TPixel>(Bitmap bitmap) where TPixel : struct, IPixel
		{
			ImageData<TPixel> data = new ImageData<TPixel>(bitmap.Width, bitmap.Height, 1);

			byte[] buffer;
			int height;
			int stride;
			GetBytes(bitmap, out buffer, out height, out stride);

			for (int row = 0, dataRowIndex = 0; row < height; row++, dataRowIndex += stride)
			{
				for (int column = 0, dataIndex = dataRowIndex; column < bitmap.Width; column++, dataIndex+=ImageData<TPixel>.PixelSize)
				{
					TPixel pixel = new TPixel();
					pixel.CopyFrom(buffer, dataIndex);
					data.Buffer.Set(column, row, 0, pixel);
				}
			}
			return data;
		}

		public static T Get<T>(this T[, ,] buffer, int column, int row, int depth)
		{
			return buffer[depth, row, column];
		}

		public static void Set<T>(this T[, ,] buffer, int column, int row, int depth, T value)
		{
			buffer[depth, row, column] = value;
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
			return image.Size.Width;
		}

		public static int Height(this IImage image)
		{
			return image.Size.Height;
		}

		public static int Depth(this IImage image)
		{
			return image.Size.Depth;
		}
	}
}
