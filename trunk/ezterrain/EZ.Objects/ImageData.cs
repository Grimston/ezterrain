using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OpenTK.Graphics;

namespace EZ.Objects
{
	public class ImageData<TPixel> : IImageData
		where TPixel : struct, IPixel
	{
		public static readonly int PixelSize = Marshal.SizeOf(typeof(TPixel));

		public ImageData(int width, int height, int depth)
		{
			this.Size = new Size3D(width, height, depth);
			this.Buffer = new TPixel[Size.Width, Size.Height, Size.Depth];
		}

		public TPixel[,,] Buffer { get; private set; }

		public Size3D Size { get; private set; }

		public void CopyTo(ImageData<TPixel> data, CopyInfo info)
		{
			for (int d = 0; d < info.Size.Depth; d++)
			{
				for (int r = 0; r < info.Size.Height; r++)
				{
					for (int c = 0; c < info.Size.Width; c++)
					{
						data.Buffer[c + info.Destination.Column,
									r + info.Destination.Row,
									d + info.Destination.Depth] = Buffer[c + info.Source.Column,
																			  r + info.Source.Row,
																			  d + info.Source.Depth];
					}
				}
			}
		}

		void IImageData.CopyTo(IImageData data, CopyInfo info)
		{
			CopyTo((ImageData<TPixel>)data, info);
		}

		IImageData IImageData.this[Region3D region]
		{
			get { return this[region]; }
			set { this[region] = (ImageData<TPixel>)value; }
		}

		public ImageData<TPixel> this[Region3D region]
		{
			get
			{
				ImageData<TPixel> data = new ImageData<TPixel>(region.Size.Width,
																region.Size.Height,
																region.Size.Depth);

				for (int d = 0; d < region.Size.Depth; d++)
				{
					for (int r = 0; r < region.Size.Height; r++)
					{
						for (int c = 0; c < region.Size.Width; c++)
						{
							data.Buffer[c, r, d] = Buffer[c + region.Index.Column,
														  r + region.Index.Row,
														  d + region.Index.Depth];
						}
					}
				}

				return data;
			}

			set
			{
				for (int d = 0; d < region.Size.Depth; d++)
				{
					for (int r = 0; r < region.Size.Height; r++)
					{
						for (int c = 0; c < region.Size.Width; c++)
						{
							Buffer[c + region.Index.Column,
								   r + region.Index.Row,
								   d + region.Index.Depth] = value.Buffer[c, r, d];
						}
					}
				}
			}
		}
	}
}
