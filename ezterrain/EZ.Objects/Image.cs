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

		private ImageData<TPixel> data;

		protected Image(TextureTarget target, ImageData<TPixel> data)
		{
			this.DirtyRegions = new List<Region3D>();
			this.target = target;
			this.data = data;
		}

		public ImageData<TPixel> this[Region3D region]
		{
			get { return data[region]; }
			set
			{
				data[region] = value;
				DirtyRegions.Add(region);
			}
		}

		IImageData IImage.this[Region3D region]
		{
			get { return this[region]; }
			set { this[region] = (ImageData<TPixel>)value; }
		}

		private TextureTarget target;
		public TextureTarget Target
		{
			get { return target; }
			set
			{
				if (target != value)
				{
					target = value;
					Dirty();
				}
			}
		}

		public void CopyTo(Image<TPixel> image, CopyInfo copyInfo)
		{
			data.CopyTo(image.data, copyInfo);
			image.DirtyRegions.Add(copyInfo.DestinationRegion);
		}

		void IImage.CopyTo(IImage image, CopyInfo copyInfo)
		{
			CopyTo((Image<TPixel>)image, copyInfo);
		}

		public ICollection<Region3D> DirtyRegions { get; private set; }

		public void Dirty()
		{
			DirtyRegions.Clear();
			DirtyRegions.Add(Bounds);
		}

		public Region3D Bounds
		{
			get { return new Region3D(Index3D.Empty, Size); }
		}

		public Size3D Size
		{
			get { return data.Size; }
		}

		protected TPixel[, ,] Buffer
		{
			get { return data.Buffer; }
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

		IImage IImage.NewImage(Size3D size)
		{
			return NewImage(size);
		}

		protected abstract Image<TPixel> NewImage(Size3D size);
	}
}
