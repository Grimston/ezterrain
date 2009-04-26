using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EZ.Renderer;
using Ez.Clipmaps;

namespace Ez.Application
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			rendererControl.Renderables.Add(new FPSDisplay());
			rendererControl.Renderables.Add(new Clipmap(257));
		}

		private void refreshTimer_Tick(object sender, EventArgs e)
		{
			rendererControl.Refresh();
		}
	}
}
