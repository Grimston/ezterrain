using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;

namespace EZ.Renderer
{
	public partial class RendererControl : GLControl
	{
		public RendererControl()
		{
			InitializeComponent();
		}

		#region Load
		protected bool Loaded { get; private set; }

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			Loaded = true;

			SetClearColor();
		}
		#endregion

		#region ClearColor
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);

			SetClearColor();
		}

		private void SetClearColor()
		{
			if (Loaded)
			{
				GL.ClearColor(BackColor);
			}
		}
		#endregion

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if (Loaded && Width > 0 && Height > 0)
			{
				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadIdentity();
				GL.Viewport(0, 0, Width, Height);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();


			SwapBuffers();
		}
	}
}
