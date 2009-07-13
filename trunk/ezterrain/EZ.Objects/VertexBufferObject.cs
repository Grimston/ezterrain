using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using System.Runtime.InteropServices;

namespace EZ.Objects
{
	public class VertexBufferObject<T> : Disposable where T : struct
	{
		#region Vertex information
		public static readonly int ElementSize = Marshal.SizeOf(typeof(T));

		public static readonly VertexPointerInfo DefaultPointer;

		static VertexBufferObject()
		{
			VertexAttribute attribute = GetAttribute<VertexAttribute>(typeof(T));

			if (attribute != null)
			{
				DefaultPointer = new VertexPointerInfo(attribute.CoordinateCount, 
														attribute.PointerType, 
														attribute.Stride, 
														attribute.Offset);
			}
		}

		private static TAttribute GetAttribute<TAttribute>(Type type)
		{
			Type attributeType = typeof(TAttribute);
			foreach (TAttribute attribute
						in type.GetCustomAttributes(attributeType, false))
			{
				return attribute;
			}

			return default(TAttribute);
		}
		#endregion

		public VertexBufferObject(EnableCap arrayType,
									BufferTarget target,
									BufferUsageHint usage, 
									VertexPointerInfo pointer)
		{
			Buffer = new List<T>();

			this.ArrayType = arrayType;
			this.BufferTarget = target;
			this.Pointer = pointer;
			this.Usage = usage;
		}

		public VertexBufferObject(EnableCap arrayType, BufferTarget target, BufferUsageHint usage)
			: this(arrayType, target, usage, DefaultPointer)
		{ }

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

		public List<T> Buffer { get; private set; }

		public BufferTarget BufferTarget { get; private set; }

		public BufferUsageHint Usage { get; set; }

		public VertexPointerInfo Pointer { get; private set; }

		public EnableCap ArrayType { get; private set; }

		public void Bind()
		{
			GL.Enable(ArrayType);
			GL.BindBuffer(BufferTarget, Handle);
		}

		public void Upload()
		{
			if (Buffer.Count > 0)
			{
				GL.BufferData(BufferTarget,
							  new IntPtr(Buffer.Count * ElementSize),
							  IntPtr.Zero,
							  Usage);
				GL.BufferSubData(BufferTarget,
								 IntPtr.Zero,
								 (IntPtr)(Buffer.Count * ElementSize),
								 Buffer.ToArray());
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
