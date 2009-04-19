using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using OpenTK.Graphics;
using EZ.Core;

namespace EZ.Objects
{
	public class VertexBuffer<T> : Collection<T> where T : struct
	{
		static VertexBuffer()
		{
			ElementSize = Marshal.SizeOf(typeof(T));
		}

		public static int ElementSize { get; private set; }

		public VertexBuffer()
		{
			DirtyRange = Range.Invalid;
		}

		public VertexBuffer(IList<T> list)
			: base(list)
		{
			DirtyRange = new Range(0, list.Count);
		}

		#region Dirty
		public Range DirtyRange { get; set; }

		protected override void ClearItems()
		{
			base.ClearItems();
			DirtyRange = Range.Invalid;
		}

		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);
			DirtyRange.Expand(new Range(index, Count - 1));
		}

		protected override void RemoveItem(int index)
		{
			base.RemoveItem(index);
			DirtyRange.Expand(new Range(index, Count - 1));
		}

		protected override void SetItem(int index, T item)
		{
			base.SetItem(index, item);
			DirtyRange.Expand(index);
		}
		#endregion

		public T[] DirtyData
		{
			get
			{
				T[] data = new T[DirtyRange.Length];

				for (int i = 0; i < data.Length; i++)
				{
					data[i] = this[DirtyRange.Min + i];
				}

				return data;
			}
		}
	}
}
