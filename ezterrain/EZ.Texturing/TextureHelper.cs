using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using System.Runtime.InteropServices;
using EZ.Imaging;
using System.Drawing;

namespace EZ.Texturing
{
	public static class TextureHelper
	{
		public static EZ.Imaging.Image Get2DImage(string fileName)
		{
			Bitmap bitmap = (Bitmap)Bitmap.FromFile(fileName);
			switch (bitmap.PixelFormat)
			{
				case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
					return new TexImage2D<BGR>(GetImageData<BGR>(bitmap));
				case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
					return new TexImage2D<BGRA>(GetImageData<BGRA>(bitmap));
				default:
					throw new NotImplementedException();
			}
		}

		public static TexImageArray GetImageArray(string fileNameFormat, int count, Size2D size)
		{
			EZ.Imaging.Image[] images = new EZ.Imaging.Image[count];

			for (int i = 0; i < images.Length; i++)
			{
				images[i] = GetArrayImage(string.Format(fileNameFormat, i), i);
				images[i].Bounds = new Region2D(Index2D.Empty, size);
			}

			return new TexImageArray(size, images);
		}

		private static EZ.Imaging.Image GetArrayImage(string fileName, int index)
		{
			Bitmap bitmap = (Bitmap)Bitmap.FromFile(fileName);
			switch (bitmap.PixelFormat)
			{
				case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
					return new TexArrayImage2D<BGR>(index, GetImageData<BGR>(bitmap));
				case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
					return new TexArrayImage2D<BGRA>(index, GetImageData<BGRA>(bitmap));
				default:
					throw new NotImplementedException();
			}
		}

		private static TPixel[,] GetImageData<TPixel>(Bitmap bitmap) where TPixel : struct, IPixel
		{
			TPixel[,] data = new TPixel[bitmap.Height, bitmap.Width];

			byte[] buffer;
			int height;
			int stride;
			GetBytes(bitmap, out buffer, out height, out stride);

			int pixelSize = Marshal.SizeOf(typeof(TPixel));
			int width = stride / pixelSize;

			for (int row = 0, dataRowIndex = 0; row < height; row++, dataRowIndex += stride)
			{
				for (int column = 0, dataIndex = dataRowIndex;
					column < width;
					column++, dataIndex += pixelSize)
				{
					data[row, column].CopyFrom(buffer, dataIndex);
				}
			}
			return data;
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
	}
}
