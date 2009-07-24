using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;

namespace EZ.Texturing
{
	[global::System.AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	sealed class PixelAttribute : Attribute
	{
		public PixelAttribute(PixelInternalFormat internalFormat, PixelFormat format, PixelType type)
		{
			this.InternalFormat = internalFormat;
			this.Format = format;
			this.Type = type;
		}

		public PixelInternalFormat InternalFormat { get; private set; }

		public PixelFormat Format { get; private set; }

		public PixelType Type { get; private set; }
	}
}
