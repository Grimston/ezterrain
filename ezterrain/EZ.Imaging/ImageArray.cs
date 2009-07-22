using System;

namespace EZ.Imaging
{
	public class ImageArray<T> : Image
	{
		public ImageArray(Size2D size, int depth)
			: base(size)
		{
			
		}
		
		public Image2D<T> this[int index]
		{
			get; 
			private set; 
		}
	}
}
