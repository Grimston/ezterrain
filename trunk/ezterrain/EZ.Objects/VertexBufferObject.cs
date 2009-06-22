using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;

namespace EZ.Objects
{
	public class VertexBufferObject<T> : Disposable where T : struct
	{
		public VertexBufferObject()
		{
			Buffer = new VertexBuffer<T>();
		}

		public VertexBufferObject(IList<T> list)
		{
			Buffer = new VertexBuffer<T>(list);
		}

		#region Dispose
		protected override void Dispose(bool nongc)
		{
			if (nongc)
			{
				if (Handle > 0)
				{
					Delete();
				}
			}
		}
		#endregion

		#region Handle
		private int handle;
		public int Handle
		{
			get { return handle; }
		}

		public void Create()
		{
			GL.GenBuffers(1, out handle);
		}

		public void Delete()
		{
			GL.DeleteBuffers(1, ref handle);
		}

		#endregion

		public VertexBuffer<T> Buffer { get; private set; }

		public BufferTarget BufferTarget { get; set; }

		public BufferUsageHint Usage { get; set; }

		public VertexPointerInfo Pointer { get; set; }

		public void Bind()
		{
			GL.Enable(EnableCap.VertexArray);
			GL.BindBuffer(BufferTarget, Handle);
		}

		public void Upload()
		{
			if (Buffer.DirtyRange.IsValid
			 && Buffer.DirtyRange.Length > 0 
			 && Buffer.Count > 0)
			{
				GL.BufferData(BufferTarget,
							  new IntPtr(Buffer.Count * VertexBuffer<T>.ElementSize),
							  IntPtr.Zero,
							  Usage);
				GL.BufferSubData(BufferTarget,
								 (IntPtr)(Buffer.DirtyRange.Min * VertexBuffer<T>.ElementSize),
								 (IntPtr)(Buffer.DirtyRange.Length * VertexBuffer<T>.ElementSize),
								 Buffer.DirtyData);

				Buffer.DirtyRange = EZ.Core.Range.Invalid;
			}
		}

		public void SetPointer()
		{
			GL.VertexPointer(Pointer.CoordinateCount,
							 Pointer.PointerType,
							 Pointer.Stride,
							 Pointer.Offset);
		}

		public void Unbind()
		{
			GL.BindBuffer(BufferTarget, 0);
		}
	}
}
