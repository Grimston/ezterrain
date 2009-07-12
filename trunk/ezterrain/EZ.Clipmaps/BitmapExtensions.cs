using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using EZ.Objects;

namespace Ez.Clipmaps
{
	public static class BitmapExtensions
	{
		public static void CopyPartsTo(this IImage source, TextureArrayElement destination, CopyInfo copyInfo)
		{
			foreach (CopyInfo info in GetCopyInfo(source.Size, destination.Image.Size, copyInfo))
			{
				source.CopyTo(destination.Image, info);
			}
		}

		private static IEnumerable<CopyInfo> GetCopyInfo(Size3D srcBounds, Size3D dstBounds, CopyInfo copyInfo)
		{
			return from dstInfo in GetDestinationCopyInfo(dstBounds, copyInfo)
				   from dstSrcInfo in GetSourceCopyInfo(srcBounds, dstInfo)
				   select dstSrcInfo;
		}

		private static IEnumerable<CopyInfo> GetDestinationCopyInfo(Size3D dstBounds, CopyInfo copyInfo)
		{
			return from info in GetDividedCopyInfo(dstBounds, new TmpCopyInfo(copyInfo.Destination, copyInfo.Source, copyInfo.Size))
				   select new CopyInfo(info.Offset2, info.Offset1, info.Size);
		}

		private static IEnumerable<CopyInfo> GetSourceCopyInfo(Size3D srcBounds, CopyInfo copyInfo)
		{
			return from info in GetDividedCopyInfo(srcBounds, new TmpCopyInfo(copyInfo.Source, copyInfo.Destination, copyInfo.Size))
				   select new CopyInfo(info.Offset1, info.Offset2, info.Size);
		}

		private static IEnumerable<TmpCopyInfo> GetDividedCopyInfo(Size3D bounds, TmpCopyInfo copyInfo)
		{
			copyInfo.Offset1.Column = Repeat(copyInfo.Offset1.Column, bounds.Width);
			copyInfo.Offset1.Row = Repeat(copyInfo.Offset1.Row, bounds.Height);

			Index3D copyLowerRight = copyInfo.Offset1;
			copyLowerRight.Column += copyInfo.Size.Width;
			copyLowerRight.Row += copyInfo.Size.Height;

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

				Index3D midPoint = new Index3D(Math.Min(bounds.Width, copyLowerRight.Column),
												Math.Min(bounds.Height, copyLowerRight.Row), 0);

				#region Region1
				Size3D region1Size = new Size3D(midPoint.Column - copyInfo.Offset1.Column, midPoint.Row - copyInfo.Offset1.Row, 1);
				Index3D region1Offset1 = copyInfo.Offset1;
				Index3D region1Offset2 = copyInfo.Offset2;
				TmpCopyInfo region1Info = new TmpCopyInfo(region1Offset1, region1Offset2, region1Size);

				if (region1Size.Width > 0
				 && region1Size.Height > 0)
				{
					yield return region1Info;
				}
				#endregion

				#region Region2
				Size3D region2Size = new Size3D(copyInfo.Size.Width - region1Size.Width, region1Size.Height, 1);
				Index3D region2Offset1 = new Index3D(0, region1Offset1.Row, 0);
				Index3D region2Offset2 = new Index3D(region1Offset2.Column + region1Size.Width, region1Offset2.Row, 0);
				TmpCopyInfo region2Info = new TmpCopyInfo(region2Offset1, region2Offset2, region2Size);

				if (region2Size.Width > 0
				 && region2Size.Height > 0)
				{
					yield return region2Info;
				}
				#endregion

				#region Region3
				Size3D region3Size = new Size3D(region1Size.Width, copyInfo.Size.Height - region1Size.Height, 1);
				Index3D region3Offset1 = new Index3D(region1Offset1.Column, 0, 0);
				Index3D region3Offset2 = new Index3D(region1Offset2.Column, region1Offset2.Row + region1Size.Height, 0);
				TmpCopyInfo region3Info = new TmpCopyInfo(region3Offset1, region3Offset2, region3Size);

				if (region3Size.Width > 0
				 && region3Size.Height > 0)
				{
					yield return region3Info;
				}
				#endregion

				#region Region4
				Size3D region4Size = new Size3D(region2Size.Width, region3Size.Height, 1);
				Index3D region4Offset1 = new Index3D(region2Offset1.Column, region3Offset1.Row, 0);
				Index3D region4Offset2 = new Index3D(region2Offset2.Column, region3Offset2.Row, 0);
				TmpCopyInfo region4Info = new TmpCopyInfo(region4Offset1, region4Offset2, region4Size);

				if (region4Size.Width > 0
				 && region4Size.Height > 0)
				{
					yield return region4Info;
				}
				#endregion
			}
		}

		public static IEnumerable<Region3D> Diff(Index3D oldPosition, Index3D newPosition, Size3D size)
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

			Index3D oldBottomRight = new Index3D(oldPosition.Column + size.Width,
												 oldPosition.Row + size.Height, 0);

			Index3D newBottomRight = new Index3D(newPosition.Column + size.Width,
												 newPosition.Row + size.Height, 0);

			if (oldPosition.Column < newPosition.Column)
			{
				if (oldPosition.Row < newPosition.Row)
				{
					#region
					Region3D r2 = Region3D.FromLTNRBF(oldBottomRight.Column, newPosition.Row, 0, newBottomRight.Column, oldBottomRight.Row, 1);

					if (r2.Width > 0 && r2.Height > 0)
					{
						yield return r2;
					}

					Region3D r3 = Region3D.FromLTNRBF(newPosition.Column, oldBottomRight.Row, 0, oldBottomRight.Column, newBottomRight.Row, 1);

					if (r3.Width > 0 && r3.Height > 0)
					{
						yield return r3;
					}

					Region3D r4 = new Region3D(r2.Column, r3.Row, 0, r2.Width, r3.Height, 1);

					if (r4.Width > 0 && r4.Height > 0)
					{
						yield return r4;
					}
					#endregion
				}
				else
				{
					#region
					Region3D r1 = Region3D.FromLTNRBF(newPosition.Column, newPosition.Row, 0, oldBottomRight.Column, oldPosition.Row, 1);

					if (r1.Width > 0 && r1.Height > 0)
					{
						yield return r1;
					}

					Region3D r4 = Region3D.FromLTNRBF(oldBottomRight.Column, oldPosition.Row, 0, newBottomRight.Column, newBottomRight.Row, 1);
					if (r4.Width > 0 && r4.Height > 0)
					{
						yield return r4;
					}

					Region3D r2 = new Region3D(r4.Column, r1.Row, 0, r4.Width, r1.Height, 1);

					if (r2.Width > 0 && r2.Height > 0)
					{
						yield return r2;
					}
					#endregion
				}
			}
			else
			{
				if (oldPosition.Row < newPosition.Row)
				{
					#region
					Region3D r1 = Region3D.FromLTNRBF(newPosition.Column, newPosition.Row, 0, oldPosition.Column, oldBottomRight.Row, 1);

					if (r1.Width > 0 && r1.Height > 0)
					{
						yield return r1;
					}

					Region3D r4 = Region3D.FromLTNRBF(oldPosition.Column, oldBottomRight.Row, 0, newBottomRight.Column, newBottomRight.Row, 1);

					if (r4.Width > 0 && r4.Height > 0)
					{
						yield return r4;
					}

					Region3D r3 = new Region3D(r1.Column, r4.Row, 0, r1.Width, r4.Height, 1);

					if (r3.Width > 0 && r3.Height > 0)
					{
						yield return r3;
					}
					#endregion
				}
				else
				{
					#region
					Region3D r2 = Region3D.FromLTNRBF(oldPosition.Column, newPosition.Row, 0, newBottomRight.Column, oldPosition.Row, 1);

					if (r2.Width > 0 && r2.Height > 0)
					{
						yield return r2;
					}

					Region3D r3 = Region3D.FromLTNRBF(newPosition.Column, oldPosition.Row, 0, oldPosition.Column, newBottomRight.Row, 1);

					if (r3.Width > 0 && r3.Height > 0)
					{
						yield return r3;
					}

					Region3D r1 = new Region3D(r3.Column, r2.Row, 0, r3.Width, r2.Height, 1);

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

		public static bool Contains(this Size3D bounds, Index3D index)
		{
			return index.Column <= bounds.Width
				&& index.Row <= bounds.Height
				&& index.Column >= 0
				&& index.Row >= 0;
		}

		public static Region3D GetCenteredRegion(this Index3D center, int width)
		{
			return new Region3D(center.Column - width / 2,
								 center.Row - width / 2, 0,
								 width, width, 1);
		}
	}
}
