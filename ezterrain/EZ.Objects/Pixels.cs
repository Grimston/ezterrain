﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using System.Runtime.InteropServices;

namespace EZ.Objects
{
	#region RGBA
	[Pixel(PixelInternalFormat.Rgba,PixelFormat.Rgba,PixelType.UnsignedByte)]
	[StructLayout(LayoutKind.Sequential)]
	public struct RGBA : IPixel
	{
		public byte R;
		public byte G;
		public byte B;
		public byte A;


		#region IPixel Members

		public void CopyFrom(byte[] data, int index)
		{
			R = data[index];
			G = data[index + 1];
			B = data[index + 2];
			A = data[index + 3];
		}

		public void CopyTo(byte[] data, int index)
		{
			data[index] = R;
			data[index + 1] = G;
			data[index + 2] = B;
			data[index + 3] = A;
		}

		#endregion
	}
	#endregion

	#region BGRA
	[Pixel(PixelInternalFormat.Rgba,PixelFormat.Bgra,PixelType.UnsignedByte)]
	[StructLayout(LayoutKind.Sequential)]
	public struct BGRA : IPixel
	{
		public byte B;
		public byte G;
		public byte R;
		public byte A;


		#region IPixel Members

		public void CopyFrom(byte[] data, int index)
		{
			B = data[index];
			G = data[index + 1];
			R = data[index + 2];
			A = data[index + 3];
		}

		public void CopyTo(byte[] data, int index)
		{
			data[index] = B;
			data[index + 1] = G;
			data[index + 2] = R;
			data[index + 3] = A;
		}

		#endregion
	}
	#endregion

	#region RGB
	[Pixel(PixelInternalFormat.Rgb,PixelFormat.Rgb,PixelType.UnsignedByte)]
	[StructLayout(LayoutKind.Sequential)]
	public struct RGB : IPixel
	{
		public byte R;
		public byte G;
		public byte B;


		#region IPixel Members

		public void CopyFrom(byte[] data, int index)
		{
			R = data[index];
			G = data[index + 1];
			B = data[index + 2];
		}

		public void CopyTo(byte[] data, int index)
		{
			data[index] = R;
			data[index + 1] = G;
			data[index + 2] = B;
		}

		#endregion
	}
	#endregion

	#region BGR
	[Pixel(PixelInternalFormat.Rgb,PixelFormat.Bgr,PixelType.UnsignedByte)]
	[StructLayout(LayoutKind.Sequential)]
	public struct BGR : IPixel
	{
		public byte B;
		public byte G;
		public byte R;


		#region IPixel Members

		public void CopyFrom(byte[] data, int index)
		{
			B = data[index];
			G = data[index + 1];
			R = data[index + 2];
		}

		public void CopyTo(byte[] data, int index)
		{
			data[index] = B;
			data[index + 1] = G;
			data[index + 2] = R;
		}

		#endregion
	}
	#endregion
}