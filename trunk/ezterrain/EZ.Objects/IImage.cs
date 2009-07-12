using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;

namespace EZ.Objects
{
	public interface IImage
	{
		void Dirty();

		IImageData this[Region3D region] { get; set; }

		Size3D Size { get; }

		void CopyTo(IImage destination, CopyInfo copyInfo);

		void Update();
	}
}
