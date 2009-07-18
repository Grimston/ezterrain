using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Objects
{
	public struct Binder<TBound> : IDisposable
		where TBound : IBound
	{
		private TBound bound;

		public Binder(TBound bound)
		{
			this.bound = bound;
			this.bound.Bind();
		}

		public void Dispose()
		{
			bound.Unbind();
		}
	}

	public struct EnumerableBinder : IDisposable
	{
		private IEnumerable<IBound> bound;

		public EnumerableBinder(IEnumerable<IBound> bound)
		{
			this.bound = bound;
			this.bound.Bind();
		}

		public void Dispose()
		{
			bound.Unbind();
		}
	}
}
