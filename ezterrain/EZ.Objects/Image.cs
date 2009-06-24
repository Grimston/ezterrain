using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using OpenTK.Graphics;

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
			get { return PixelFormat.GetComponentCount(); }
		}
	}
}
