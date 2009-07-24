using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Imaging;
using OpenTK.Graphics;

namespace EZ.Texturing
{
	internal static class Pixel<T> where T: struct, IPixel
	{
		#region Pixel Attributes
		public static readonly PixelFormat PixelFormat;
		public static readonly PixelInternalFormat PixelInternalFormat;
		public static readonly PixelType PixelType;

		static Pixel()
		{
			PixelAttribute pixel = GetAttribute<PixelAttribute>(typeof(T));

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
		#endregion
	}
}
