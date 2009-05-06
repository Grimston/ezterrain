using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Core
{
	public struct Pair<T, U>
	{
		public Pair(T t, U u)
		{
			this.t = t;
			this.u = u;
		}

		private T t;
		public T Value1
		{
			get { return t; }
			set { t = value; }
		}

		private U u;
		public U Value2
		{
			get { return u; }
			set { u = value; }
		}
	}
}