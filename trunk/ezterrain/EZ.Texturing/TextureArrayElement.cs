using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using EZ.Imaging;

namespace EZ.Texturing
{
	public class TextureArrayElement : Texture, IComparable<TextureArrayElement>
	{
		internal TextureArrayElement(int index, Image image)
			: base(image)
		{
			this.Index = index;
		}

		public int Index { get; private set; }

		public override void Initialize()
		{
			if (!Initialized)
			{
				Initialized = true;
			}
		}

		public override void Update()
		{
			throw new NotImplementedException();
		}

		//protected override void Upload(Rectangle region, BitmapData data)
		//{
		//    if (data.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
		//    {
		//        if (data.PixelFormat.GetComponentCount() == data.Stride / data.Width)
		//        {
		//            GL.TexSubImage3D(TextureTarget.Texture2DArray, 0,
		//                                 0, 0, Index,
		//                                 data.Width, data.Height, 1,
		//                                 OpenTK.Graphics.PixelFormat.Bgr,
		//                                 PixelType.UnsignedByte,
		//                                 data.Scan0);
		//        }
		//        else
		//        {
		//            for (int i = 0; i < data.Height; i++)
		//            {
		//                GL.TexSubImage3D(TextureTarget.Texture2DArray, 0,
		//                                 region.X, region.Y + i, Index,
		//                                 data.Width, 1, 1,
		//                                 OpenTK.Graphics.PixelFormat.Bgr,
		//                                 PixelType.UnsignedByte,
		//                                 new IntPtr(data.Scan0.ToInt32() + (i * data.Stride)));
		//            }
		//        }
		//    }
		//}

		#region IComparable<TextureArrayElement> Members

		public int CompareTo(TextureArrayElement other)
		{
			return Index.CompareTo(other.Index);
		}

		#endregion
	}
}
