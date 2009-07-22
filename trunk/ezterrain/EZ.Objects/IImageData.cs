using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Imaging;

namespace EZ.Objects
{
	public interface IImageData
	{
		Size3D Size { get; }

		IImageData this[Region3D region] { get; set; }

		void CopyTo(IImageData destination, CopyInfo copyInfo);
	}
}
