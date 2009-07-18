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
}
