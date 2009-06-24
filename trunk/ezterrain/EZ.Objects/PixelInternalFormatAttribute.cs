using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;

namespace EZ.Objects
{
	[global::System.AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	public sealed class PixelInternalFormatAttribute : Attribute
	{
		public PixelInternalFormatAttribute(PixelInternalFormat pixelInternalFormat)
		{
			this.PixelInternalFormat = pixelInternalFormat;
		}

		public PixelInternalFormat PixelInternalFormat { get; private set; }
	}
}
