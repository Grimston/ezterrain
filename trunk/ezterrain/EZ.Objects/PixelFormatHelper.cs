using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EZ.Objects
{
	public static class PixelFormatHelper
	{
		public static int GetComponentCount(this OpenTK.Graphics.PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case OpenTK.Graphics.PixelFormat.AbgrExt:
				case OpenTK.Graphics.PixelFormat.Bgra:
				case OpenTK.Graphics.PixelFormat.BgraInteger:
				case OpenTK.Graphics.PixelFormat.CmykExt:
				case OpenTK.Graphics.PixelFormat.CmykaExt:
				case OpenTK.Graphics.PixelFormat.DepthStencil:
				case OpenTK.Graphics.PixelFormat.Rgba:
				case OpenTK.Graphics.PixelFormat.RgbaInteger:
					return 4;
				case OpenTK.Graphics.PixelFormat.Bgr:
				case OpenTK.Graphics.PixelFormat.BgrInteger:
				case OpenTK.Graphics.PixelFormat.DepthComponent:
				case OpenTK.Graphics.PixelFormat.Rgb:
				case OpenTK.Graphics.PixelFormat.RgbInteger:
				case OpenTK.Graphics.PixelFormat.Ycrcb444Sgix:
					return 3;
				case OpenTK.Graphics.PixelFormat.LuminanceAlpha:
				case OpenTK.Graphics.PixelFormat.Rg:
				case OpenTK.Graphics.PixelFormat.RgInteger:
				case OpenTK.Graphics.PixelFormat.Ycrcb422Sgix:
					return 2;
				case OpenTK.Graphics.PixelFormat.Alpha:
				case OpenTK.Graphics.PixelFormat.AlphaInteger:
				case OpenTK.Graphics.PixelFormat.Blue:
				case OpenTK.Graphics.PixelFormat.BlueInteger:
				case OpenTK.Graphics.PixelFormat.ColorIndex:
				case OpenTK.Graphics.PixelFormat.Green:
				case OpenTK.Graphics.PixelFormat.GreenInteger:
				case OpenTK.Graphics.PixelFormat.Luminance:
				case OpenTK.Graphics.PixelFormat.Red:
				case OpenTK.Graphics.PixelFormat.RedInteger:
				case OpenTK.Graphics.PixelFormat.StencilIndex:
					return 1;
				default:
					throw new InvalidEnumArgumentException(pixelFormat.ToString());
			}
		}

		public static int GetComponentCount(this System.Drawing.Imaging.PixelFormat pixelFormat)
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
