using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;

namespace EZ.Objects
{
	[global::System.AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	public sealed class PixelFormatAttribute : Attribute
	{
		public PixelFormatAttribute(PixelFormat pixelFormat)
		{
			this.PixelFormat = pixelFormat;
		}

		public PixelFormat PixelFormat { get; private set; }
	}
}
