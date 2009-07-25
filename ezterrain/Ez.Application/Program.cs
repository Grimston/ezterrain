using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EZ.Renderer;
using Ez.Clipmaps;

namespace Ez.Application
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			//System.Windows.Forms.Application.EnableVisualStyles();
			//System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			//System.Windows.Forms.Application.Run(new MainForm());

			using (RendererWindow renderer = new RendererWindow())
			{
				renderer.Renderables.Add(new FPSDisplay());
				renderer.Renderables.Add(new BasicTerrain(513));
				renderer.Run();
			}
		}
	}
}
