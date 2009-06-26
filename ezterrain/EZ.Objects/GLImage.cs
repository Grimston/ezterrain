using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using OpenTK.Graphics;

namespace EZ.Objects
{
	public abstract class GlImage<TData, TPixel>
		where TData : ImageData<TPixel>
		where TPixel : struct, IPixel
	{
		public static readonly PixelFormat PixelFormat;
		public static readonly PixelInternalFormat PixelInternalFormat;
		public static readonly PixelType PixelType;

		static GlImage()
		{
			GlPixelAttribute pixel = GetAttribute<GlPixelAttribute>(typeof(TPixel));

			PixelInternalFormat = pixel.InternalFormat;
			PixelFormat = pixel.Format;
			PixelType = pixel.Type;
		}

		private static TAttribute GetAttribute<TAttribute>(Type type)
		{
			Type attributeType = typeof(TAttribute);
			foreach (TAttribute attribute
						in type.GetCustomAttributes(attributeType, false))
			{
				return attribute;
			}

			throw new ArgumentException("Given type should have " + attributeType, "type:" + type);
		}

		protected GlImage(TData data)
		{
			this.Data = data;
		}

		public TData Data { get; private set; }
	}
}
