using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Core
{
	public enum RenderGroup : uint
	{
		HUD = uint.MaxValue,
		Transparent = uint.MaxValue / 2,
		Opaque = 0
	}
}
