using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Objects
{
	public abstract class Disposable : IDisposable
	{
		public Disposable()
		{ }

		~Disposable()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected abstract void Dispose(bool nongc);
	}
}
