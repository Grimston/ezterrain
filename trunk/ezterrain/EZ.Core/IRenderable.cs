using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Core
{
	public interface IRenderable
	{
		RenderGroup RenderGroup { get; }

		bool Initialized { get; }

		void Initialize();

		bool Update(RenderInfo info);

		void Render(RenderInfo info);
	}
}
