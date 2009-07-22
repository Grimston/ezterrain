using System;
using System.Runtime.InteropServices;

namespace EZ.Imaging
{
	public class PinnedImage : IDisposable
	{
		private Image owner;
		private GCHandle handle;
		
		internal PinnedImage(Image owner, GCHandle handle)
		{
			this.owner = owner;
		}
		
		#region Disposal
		~PinnedImage()
		{
			Dispose(false);
		}
		
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		private bool disposed;
		
		protected void Dispose(bool nongc)
		{
			if(!disposed)
			{
				handle.Free();
			}

			disposed = true;
		}
		#endregion
		
		public IntPtr Data
		{
			get{ return handle.AddrOfPinnedObject(); }
		}
	}
}
