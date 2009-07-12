using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using OpenTK.Graphics;

namespace EZ.Objects
{
	public abstract class Image<TPixel> : IImage
		where TPixel : struct, IPixel
	{
		public static readonly PixelFormat PixelFormat;
		public static readonly PixelInternalFormat PixelInternalFormat;
		public static readonly PixelType PixelType;

		static Image()
		{
			PixelAttribute pixel = GetAttribute<PixelAttribute>(typeof(TPixel));

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

		protected Image(TextureTarget target, ImageData<TPixel> data)
		{
			this.DirtyRegions = new List<Region3D>();
			this.target = target;
			this.data = data;
		}

		private ImageData<TPixel> data;
		public ImageData<TPixel> Data 
		{
			get { return data; }
			set
			{
				if (data != value)
				{
					data = value;
					Dirty();
				}
			}
		}

		IImageData IImage.Data
		{
			get { return Data; }
			set { Data = (ImageData<TPixel>)value; }
		}

		public ImageData<TPixel> GetRegion(Region3D region)
		{
			return Data[region];
		}

		IImageData IImage.GetRegion(Region3D region)
		{
			return GetRegion(region);
		}

		public void SetRegion(Region3D region, ImageData<TPixel> data)
		{
			Data[region] = data;
			DirtyRegions.Add(region);
		}

		void IImage.SetRegion(Region3D region, IImageData data)
		{
			SetRegion(region, (ImageData<TPixel>)data);
		}

		private TextureTarget target;
		public TextureTarget Target 
		{ 
			get{return target;}
			set 
			{
				if (target != value)
				{
					target = value;
					Dirty();
				}
			}
		}

		public ICollection<Region3D> DirtyRegions { get; private set; }

		public void Dirty()
		{
			DirtyRegions.Clear();
			DirtyRegions.Add(Bounds);
		}

		public Region3D Bounds
		{
			get { return new Region3D(Index3D.Empty, Data.Size); }
		}

		public void Update()
		{
			foreach (Region3D region in DirtyRegions)
			{
				Upload(region);
			}

			DirtyRegions.Clear();
		}

		protected abstract void Upload(Region3D region);
	}
}
