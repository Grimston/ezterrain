using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;

namespace EZ.Objects
{
	[global::System.AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	sealed class PixelTypeAttribute : Attribute
	{
		public PixelTypeAttribute(PixelType pixelType)
		{
			this.PixelType = pixelType;
		}

		public PixelType PixelType { get; private set; }
	}
}
