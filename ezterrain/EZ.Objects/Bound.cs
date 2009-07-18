using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Objects
{
	public static class Bound
	{
		public static Binder<TBound> Use<TBound>(this TBound bound)
			where TBound:IBound
		{
			return new Binder<TBound>(bound);
		}
	}
}
