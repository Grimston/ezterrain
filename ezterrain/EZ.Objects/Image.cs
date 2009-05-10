using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics;
using System.ComponentModel;

namespace EZ.Objects
{
	public class Image
	{
		public Image()
		{

		}

		private byte[] data;

		public Size Size { get; set; }

		public PixelFormat PixelFormat { get; set; }

		private void Allocate()
		{
			data = new byte[GetTotalSize()];
		}

		public void Reset()
		{
			if (this.data.Length == GetTotalSize())
			{
				Array.Clear(this.data, 0, this.data.Length);
			}
			else
			{
				Allocate();
			}
		}

		public void Set(byte[] data, Size size, PixelFormat pixelFormat)
		{
			this.Size = size;
			this.PixelFormat = pixelFormat;

			if (data.Length != GetTotalSize())
			{
				throw new ArgumentException("Size mismatch", "data");
			}

			Set(data);
		}

		public void Set(byte[] data)
		{
			Reset();
			Buffer.BlockCopy(data, 0, this.data, 0, data.Length);
		}

		public void Update(Rectangle rect, byte[] data)
		{
			int copySize = rect.Width * ComponentCount;

			for (int i = rect.Top; i < rect.Bottom; i++)
			{
				Buffer.BlockCopy(data, i * copySize,
								 this.data, (i * Size.Width + rect.Left) * ComponentCount,
								 copySize);
			}
		}

		public int GetTotalSize()
		{
			return ComponentCount * Size.Width * Size.Height;
		}

		public int ComponentCount
		{
			get { return GetComponentCount(PixelFormat); }
		}

		public static int GetComponentCount(PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case PixelFormat.AbgrExt:
				case PixelFormat.Bgra:
				case PixelFormat.BgraInteger:
				case PixelFormat.CmykExt:
				case PixelFormat.CmykaExt:
				case PixelFormat.DepthStencil:
				case PixelFormat.Rgba:
				case PixelFormat.RgbaInteger:
					return 4;
				case PixelFormat.Bgr:
				case PixelFormat.BgrInteger:
				case PixelFormat.DepthComponent:
				case PixelFormat.Luminance16Alpha8IccSgix:
				case PixelFormat.R5G6B5A8IccSgix:
				case PixelFormat.Rgb:
				case PixelFormat.RgbInteger:
				case PixelFormat.Ycrcb444Sgix:
					return 3;
				case PixelFormat.Alpha16IccSgix:
				case PixelFormat.Luminance16IccSgix:
				case PixelFormat.LuminanceAlpha:
				case PixelFormat.R5G6B5IccSgix:
				case PixelFormat.Rg:
				case PixelFormat.RgInteger:
				case PixelFormat.Ycrcb422Sgix:
					return 2;
				case PixelFormat.Alpha:
				case PixelFormat.AlphaInteger:
				case PixelFormat.Blue:
				case PixelFormat.BlueInteger:
				case PixelFormat.ColorIndex:
				case PixelFormat.Green:
				case PixelFormat.GreenInteger:
				case PixelFormat.Luminance:
				case PixelFormat.Red:
				case PixelFormat.RedInteger:
				case PixelFormat.StencilIndex:
					return 1;
				default:
					throw new InvalidEnumArgumentException(pixelFormat.ToString());
			}
		}

		public static int GetComponentCount(System.Drawing.Imaging.PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case System.Drawing.Imaging.PixelFormat.Alpha:
				case System.Drawing.Imaging.PixelFormat.PAlpha:
				case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
					return 1;
				case System.Drawing.Imaging.PixelFormat.Format16bppGrayScale:
				case System.Drawing.Imaging.PixelFormat.Format16bppRgb555:
				case System.Drawing.Imaging.PixelFormat.Format16bppRgb565:
				case System.Drawing.Imaging.PixelFormat.Format16bppArgb1555:
					return 2;
				case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
					return 3;
				case System.Drawing.Imaging.PixelFormat.Canonical:
				case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
				case System.Drawing.Imaging.PixelFormat.Format32bppPArgb:
				case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
					return 4;
				case System.Drawing.Imaging.PixelFormat.Format48bppRgb:
					return 6;
				case System.Drawing.Imaging.PixelFormat.Format64bppArgb:
				case System.Drawing.Imaging.PixelFormat.Format64bppPArgb:
					return 8;
				case System.Drawing.Imaging.PixelFormat.Format1bppIndexed:
				case System.Drawing.Imaging.PixelFormat.Extended:
				case System.Drawing.Imaging.PixelFormat.Format4bppIndexed:
				case System.Drawing.Imaging.PixelFormat.Gdi:
				case System.Drawing.Imaging.PixelFormat.Indexed:
				case System.Drawing.Imaging.PixelFormat.Max:
				case System.Drawing.Imaging.PixelFormat.Undefined:
				default:
					throw new InvalidEnumArgumentException(pixelFormat.ToString());
			}
		}
	}
}
