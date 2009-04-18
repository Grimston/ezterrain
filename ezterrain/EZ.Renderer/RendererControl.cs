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
using System.Diagnostics;
using EZ.Core;

namespace EZ.Renderer
{
	public partial class RendererControl : GLControl
	{
		private Stopwatch lapWatch;
		private Stopwatch globalWatch;
		private Camera camera;
		private uint frameNumber;

		public RendererControl()
		{
			InitializeComponent();

			lapWatch = new Stopwatch();
			globalWatch = new Stopwatch();
			camera = new Camera();

			frameNumber = 0;

			Renderables = new List<IRenderable>();
		}

		public List<IRenderable> Renderables { get; private set; }

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

		protected override void OnPaint(PaintEventArgs e)
		{
			Render();
		}

		#region Render
		private void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			UpdateProjection();

			#region Modelview
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			Glu.LookAt(camera.Position,
					   camera.Position + camera.Attitude.Direction,
					   camera.Attitude.Up);
			#endregion

			Render(GetRenderInfo());

			SwapBuffers();
		}

		private void Render(RenderInfo renderInfo)
		{
			foreach (IRenderable renderable in Renderables)
			{
				if (!renderable.Initialized)
				{
					renderable.Initialize();
				}
			}

			IEnumerable<IRenderable> renderList = Renderables.Where(r => r.Update(renderInfo));

			foreach (IRenderable renderable in renderList)
			{
				renderable.Render(renderInfo);
			}
		} 
		#endregion

		#region Projection
		private void UpdateProjection()
		{
			if (Width > 0 && Height > 0)
			{
				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadIdentity();

				Glu.Perspective(camera.Perspective.Angle,
								Width / (double)Height,
								camera.Perspective.Near,
								camera.Perspective.Far);

				GL.Viewport(0, 0, Width, Height);
			}
		} 
		#endregion

		#region RenderInfo
		private RenderInfo GetRenderInfo()
		{
			FrameStamp frameStamp = GetFrameStamp();

			RenderInfo renderInfo = new RenderInfo(new ViewerInfo(camera), frameStamp);
			return renderInfo;
		}

		private FrameStamp GetFrameStamp()
		{
			lapWatch.Stop();
			globalWatch.Stop();

			FrameStamp frameStamp = new FrameStamp(frameNumber++,
												   globalWatch.Elapsed.TotalSeconds,
												   lapWatch.Elapsed.TotalSeconds);

			lapWatch.Reset();
			lapWatch.Start();
			globalWatch.Start();

			return frameStamp;
		} 
		#endregion
	}
}
