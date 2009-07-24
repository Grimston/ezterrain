using System;
using System.Collections.Generic;
using System.Linq;

namespace EZ.Imaging
{
	public class ImageArray : Image, IEnumerable<Image>
	{
		private Image[] images;

		public ImageArray(Size2D size, params Image[] images)
			: base(size)
		{
			this.images = new Image[images.Length];

			images.CopyTo(this.images, 0);
		}

		public Image this[int index]
		{
			get { return images[index]; }
		}

		public int Depth
		{
			get { return images.Length; }
		}

		#region IEnumerable<Image> Members

		public IEnumerator<Image> GetEnumerator()
		{
			foreach (Image image in images)
			{
				yield return image;
			}
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
