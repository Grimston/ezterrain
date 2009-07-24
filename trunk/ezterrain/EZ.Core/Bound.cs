using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Core
{
	public static class Bound
	{
		public static Binder<TBound> Use<TBound>(this TBound bound)
			where TBound : IBound
		{
			return new Binder<TBound>(bound);
		}

		public static EnumerableBinder Use(this IEnumerable<IBound> bounds)
		{
			return new EnumerableBinder(bounds);
		}

		public static void Bind(this IEnumerable<IBound> bounds)
		{
			foreach (IBound bound in bounds)
			{
				bound.Bind();
			}
		}

		public static void Unbind(this IEnumerable<IBound> bounds)
		{
			foreach (IBound bound in bounds)
			{
				bound.Unbind();
			}
		}
	}
}
