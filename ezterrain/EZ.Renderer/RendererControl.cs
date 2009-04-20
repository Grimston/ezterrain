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
using OpenTK.Math;

namespace EZ.Renderer
{
	public partial class RendererControl : GLControl
	{
		private Stopwatch lapWatch;
		private Stopwatch globalWatch;
		private Camera camera;
		private uint frameNumber;
		private ObjectDisplay objects;

		public RendererControl()
		{
			InitializeComponent();

			lapWatch = new Stopwatch();
			globalWatch = new Stopwatch();
			camera = new Camera();

			frameNumber = 0;

			Renderables = new List<IRenderable>();

			objects = new ObjectDisplay();
			objects.Color = ForeColor;
			objects.Location = new PointF(0, 50);
			Renderables.Add(objects);
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

		#region Input Handling
		private Point mousePosition;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			mousePosition = e.Location;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			int dx = e.Location.X - mousePosition.X;
			int dy = e.Location.Y - mousePosition.Y;

			if (e.Button == MouseButtons.Right)
			{
				camera.Position += camera.Attitude.Side * dx;
				camera.Position += camera.Attitude.Up * dy;
			}
			else if (e.Button == MouseButtons.Left)
			{
				Matrix4 pitch = Matrix4.Rotate(camera.Attitude.Side,
											   (float)Math.PI * dx / Height);
				camera.Attitude = new Attitude(Vector3.TransformVector(camera.Attitude.Direction, pitch),
											   Vector3.TransformVector(camera.Attitude.Up, pitch));
			}

			mousePosition = e.Location;
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if (e.KeyChar == 'w' || e.KeyChar == 'W')
			{
				camera.Position += camera.Attitude.Direction;
			}
			else if (e.KeyChar == 's' || e.KeyChar == 'S')
			{
				camera.Position -= camera.Attitude.Direction;
			}
		}
		#endregion

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

			objects.Objects["Camera Position"] = camera.Position;
			objects.Objects["Camera Direction"] = camera.Attitude.Direction;
			objects.Objects["Camera Up"] = camera.Attitude.Up;
			objects.Objects["Camera Side"] = camera.Attitude.Side;

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
