using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Imaging;

namespace EZ.Objects
{
	public class EmptyImage : IImage
	{
		#region IImage Members

		public void Dirty()
		{
			//do nothing
		}

		public IImageData this[Region3D region]
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public Size3D Size
		{
			get { throw new NotImplementedException(); }
		}


		public IImage NewImage(Size3D size)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(IImage destination, CopyInfo copyInfo)
		{
			//do nothing
		}

		public void Update()
		{
			//do nothing
		}

		#endregion
	}
}
