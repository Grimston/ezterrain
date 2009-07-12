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

		IImageData Data { get; set; }

		IImageData GetRegion(Region3D region);

		void SetRegion(Region3D region, IImageData data);

		void Update();
	}
}
