using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using EZ.Objects;

namespace Ez.Clipmaps
{
	public static class BitmapExtensions
	{
		private static Region3D GetSourceRegion(this CopyInfo copyInfo)
		{
			return new Region3D(new Index3D(copyInfo.Source.X, copyInfo.Source.Y, 0),
								new Size3D(copyInfo.Size.Width, copyInfo.Size.Height, 1));
		}

		private static Region3D GetDestinationRegion(this CopyInfo copyInfo)
		{
			return new Region3D(new Index3D(copyInfo.Destination.X, copyInfo.Destination.Y, 0),
								new Size3D(copyInfo.Size.Width, copyInfo.Size.Height, 1));
		}

		public static void CopyTo(this IImage source, IImage destination, CopyInfo copyInfo)
		{
			IImageData data = source.GetRegion(copyInfo.GetSourceRegion());

			destination.SetRegion(copyInfo.GetDestinationRegion(), data);
		}

		public static Size Size(this IImage image)
		{
			return new Size(image.Width(), image.Height());
		}

		public static void CopyPartsTo(this IImage source, TextureArrayElement destination, CopyInfo copyInfo)
		{
			foreach (CopyInfo info in GetCopyInfo(source.Size(), destination.Image.Size(), copyInfo))
			{
				source.CopyTo(destination.Image, info);
			}
		}

		private static IEnumerable<CopyInfo> GetCopyInfo(Size srcBounds, Size dstBounds, CopyInfo copyInfo)
		{
			return from dstInfo in GetDestinationCopyInfo(dstBounds, copyInfo)
				   from dstSrcInfo in GetSourceCopyInfo(srcBounds, dstInfo)
				   select dstSrcInfo;
		}

		private static IEnumerable<CopyInfo> GetDestinationCopyInfo(Size dstBounds, CopyInfo copyInfo)
		{
			return from info in GetDividedCopyInfo(dstBounds, new TmpCopyInfo(copyInfo.Destination, copyInfo.Source, copyInfo.Size))
				   select new CopyInfo(info.Offset2, info.Offset1, info.Size);
		}

		private static IEnumerable<CopyInfo> GetSourceCopyInfo(Size srcBounds, CopyInfo copyInfo)
		{
			return from info in GetDividedCopyInfo(srcBounds, new TmpCopyInfo(copyInfo.Source, copyInfo.Destination, copyInfo.Size))
				   select new CopyInfo(info.Offset1, info.Offset2, info.Size);
		}

		private static IEnumerable<TmpCopyInfo> GetDividedCopyInfo(Size bounds, TmpCopyInfo copyInfo)
		{
			copyInfo.Offset1.X = Repeat(copyInfo.Offset1.X, bounds.Width);
			copyInfo.Offset1.Y = Repeat(copyInfo.Offset1.Y, bounds.Height);

			Point copyLowerRight = copyInfo.Offset1 + copyInfo.Size;

			if (bounds.Contains(copyLowerRight))
			{
				yield return copyInfo;
			}
			else
			{
				/* determine copy regions
				 * x____________________
				 * |     |              |
				 * |   1 |      2       |
				 * |     |              |
				 * |-----y--------------|
				 * |   3 |      4       |
				 * |_____|______________|
				 *						z
				 *						
				 * each region will have offset and size defined (ie. copy info)
				 * x : copyInfo.Source
				 * y : midPoint
				 * z : copyInfo.Source + copyInfo.Size
				 */

				Point midPoint = new Point(Math.Min(bounds.Width, copyLowerRight.X),
											Math.Min(bounds.Height, copyLowerRight.Y));

				#region Region1
				Size region1Size = new Size(midPoint.X - copyInfo.Offset1.X, midPoint.Y - copyInfo.Offset1.Y);
				Point region1Offset1 = copyInfo.Offset1;
				Point region1Offset2 = copyInfo.Offset2;
				TmpCopyInfo region1Info = new TmpCopyInfo(region1Offset1, region1Offset2, region1Size);

				if (region1Size.Width > 0
				 && region1Size.Height > 0)
				{
					yield return region1Info;
				}
				#endregion

				#region Region2
				Size region2Size = new Size(copyInfo.Size.Width - region1Size.Width, region1Size.Height);
				Point region2Offset1 = new Point(0, region1Offset1.Y);
				Point region2Offset2 = new Point(region1Offset2.X + region1Size.Width, region1Offset2.Y);
				TmpCopyInfo region2Info = new TmpCopyInfo(region2Offset1, region2Offset2, region2Size);

				if (region2Size.Width > 0
				 && region2Size.Height > 0)
				{
					yield return region2Info;
				}
				#endregion

				#region Region3
				Size region3Size = new Size(region1Size.Width, copyInfo.Size.Height - region1Size.Height);
				Point region3Offset1 = new Point(region1Offset1.X, 0);
				Point region3Offset2 = new Point(region1Offset2.X, region1Offset2.Y + region1Size.Height);
				TmpCopyInfo region3Info = new TmpCopyInfo(region3Offset1, region3Offset2, region3Size);

				if (region3Size.Width > 0
				 && region3Size.Height > 0)
				{
					yield return region3Info;
				}
				#endregion

				#region Region4
				Size region4Size = new Size(region2Size.Width, region3Size.Height);
				Point region4Offset1 = new Point(region2Offset1.X, region3Offset1.Y);
				Point region4Offset2 = new Point(region2Offset2.X, region3Offset2.Y);
				TmpCopyInfo region4Info = new TmpCopyInfo(region4Offset1, region4Offset2, region4Size);

				if (region4Size.Width > 0
				 && region4Size.Height > 0)
				{
					yield return region4Info;
				}
				#endregion
			}
		}

		public static IEnumerable<Rectangle> Diff(Point oldPosition, Point newPosition, Size size)
		{
			/*
			 * returns difference regions for two rectangles:
			 * ax___________________
			 * |     |              |
			 * |  a1 |     a2       |
			 * |     |              |
			 * |-----bx-------------|---------.
			 * |  a3 |    a4 / b1   |   b2    |
			 * |_____|_____________ay_________|
			 *       |              |         |
			 *       |      b3      |   b4    |
			 *       |______________|________by
			 *       
			 * - there are four cases, we illustrate 2 of them here:
			 * 
			 * - either returns a1, a2, a3 or b1, b2, b3
			 *     - if newRect -> a returns a1,a2,a3
			 *     - if newRect -> b returns b2,b3,b4
			 *     
			 */

			Point oldBottomRight = new Point(oldPosition.X + size.Width,
											 oldPosition.Y + size.Height);

			Point newBottomRight = new Point(newPosition.X + size.Width,
											 newPosition.Y + size.Height);

			if (oldPosition.X < newPosition.X)
			{
				if (oldPosition.Y < newPosition.Y)
				{
					#region
					Rectangle r2 = Rectangle.FromLTRB(oldBottomRight.X, newPosition.Y, newBottomRight.X, oldBottomRight.Y);

					if (r2.Width > 0 && r2.Height > 0)
					{
						yield return r2;
					}

					Rectangle r3 = Rectangle.FromLTRB(newPosition.X, oldBottomRight.Y, oldBottomRight.X, newBottomRight.Y);

					if (r3.Width > 0 && r3.Height > 0)
					{
						yield return r3;
					}

					Rectangle r4 = new Rectangle(r2.X, r3.Y, r2.Width, r3.Height);

					if (r4.Width > 0 && r4.Height > 0)
					{
						yield return r4;
					}
					#endregion
				}
				else
				{
					#region
					Rectangle r1 = Rectangle.FromLTRB(newPosition.X, newPosition.Y, oldBottomRight.X, oldPosition.Y);

					if (r1.Width > 0 && r1.Height > 0)
					{
						yield return r1;
					}

					Rectangle r4 = Rectangle.FromLTRB(oldBottomRight.X, oldPosition.Y, newBottomRight.X, newBottomRight.Y);
					if (r4.Width > 0 && r4.Height > 0)
					{
						yield return r4;
					}

					Rectangle r2 = new Rectangle(r4.X, r1.Y, r4.Width, r1.Height);

					if (r2.Width > 0 && r2.Height > 0)
					{
						yield return r2;
					}
					#endregion
				}
			}
			else
			{
				if (oldPosition.Y < newPosition.Y)
				{
					#region
					Rectangle r1 = Rectangle.FromLTRB(newPosition.X, newPosition.Y, oldPosition.X, oldBottomRight.Y);

					if (r1.Width > 0 && r1.Height > 0)
					{
						yield return r1;
					}

					Rectangle r4 = Rectangle.FromLTRB(oldPosition.X, oldBottomRight.Y, newBottomRight.X, newBottomRight.Y);

					if (r4.Width > 0 && r4.Height > 0)
					{
						yield return r4;
					}

					Rectangle r3 = new Rectangle(r1.X, r4.Y, r1.Width, r4.Height);

					if (r3.Width > 0 && r3.Height > 0)
					{
						yield return r3;
					}
					#endregion
				}
				else
				{
					#region
					Rectangle r2 = Rectangle.FromLTRB(oldPosition.X, newPosition.Y, newBottomRight.X, oldPosition.Y);

					if (r2.Width > 0 && r2.Height > 0)
					{
						yield return r2;
					}

					Rectangle r3 = Rectangle.FromLTRB(newPosition.X, oldPosition.Y, oldPosition.X, newBottomRight.Y);

					if (r3.Width > 0 && r3.Height > 0)
					{
						yield return r3;
					}

					Rectangle r1 = new Rectangle(r3.X, r2.Y, r3.Width, r2.Height);

					if (r1.Width > 0 && r1.Height > 0)
					{
						yield return r1;
					}
					#endregion
				}
			}
		}

		public static int Repeat(int index, int range)
		{
			index %= range;
			if (index < 0)
			{
				index += range;
			}

			return index;
		}

		public static float Repeat(float index, int range)
		{
			int integralIndex = (int)index;
			int repeatedIndex = Repeat(integralIndex, range);

			if (integralIndex == index)
			{
				return repeatedIndex;
			}

			//boundary check
			return repeatedIndex == (range - 1) ? index - integralIndex 
												: repeatedIndex + integralIndex;
		}

		public static bool Contains(this Size bounds, Point point)
		{
			return point.X <= bounds.Width
				&& point.Y <= bounds.Height
				&& point.X >= 0
				&& point.Y >= 0;
		}

		public static Rectangle GetBounds(this System.Drawing.Image image)
		{
			return new Rectangle(0, 0, image.Width, image.Height);
		}

		public static Rectangle GetCenteredRect(this Point center, int width)
		{
			return new Rectangle(center.X - width / 2,
								 center.Y - width / 2,
								 width, width);
		}
	}
}
