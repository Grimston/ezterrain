using System;
using System.Runtime.InteropServices;

namespace EZ.Imaging
{
	public abstract class Image2D<T> : Image, IImageData
	{
		public Image2D(Size2D size)
			: base(size)
		{
			Pixels = new T[size.Height, size.Width];
		}
		
		public T[,] Pixels { get; private set; }

		public PinnedData Pin()
		{
			GCHandle handle = GCHandle.Alloc(Pixels, GCHandleType.Pinned);
			
			return new PinnedImage(this, handle);
		}
	}
}
