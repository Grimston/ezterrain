using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using EZ.Core;
using System.Drawing;
using System.Diagnostics;
using OpenTK.Platform;
using OpenTK.Input;
using OpenTK.Math;

namespace EZ.Renderer
{
	public class RendererWindow : GameWindow
	{
		private Stopwatch lapWatch;
		private Stopwatch globalWatch;
		private Camera camera;
		private uint frameNumber;
		private ObjectDisplay objects;

		public RendererWindow()
			: base(800, 600, GraphicsMode.Default, "EZ Terrain")
		{
			Context.ErrorChecking = true;
			VSync = VSyncMode.Off;
			ClearColor = Color.LightBlue;

			lapWatch = new Stopwatch();
			globalWatch = new Stopwatch();
			camera = new Camera();

			frameNumber = 0;

			Renderables = new RenderList();

			objects = new ObjectDisplay();
			objects.Color = Color.Black;
			objects.Location = new PointF(0, 50);
			Renderables.Add(objects);
		}

		public RenderList Renderables { get; private set; }

		public Color ClearColor { get; set; }

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

		public override void OnUpdateFrame(UpdateFrameEventArgs e)
		{
			if (Keyboard[Key.Escape])
			{
				Exit();
			}

			if (Keyboard[Key.W])
			{
				camera.Position += camera.Attitude.Direction;
			}
			
			if (Keyboard[Key.S])
			{
				camera.Position -= camera.Attitude.Direction;
			}

			if (Keyboard[Key.D])
			{
				camera.Position += camera.Attitude.Side;
			}

			if (Keyboard[Key.A])
			{
				camera.Position -= camera.Attitude.Side;
			}

			if (Keyboard[Key.U])
			{
				camera.Position += camera.Attitude.Up;
			}

			if (Keyboard[Key.J])
			{
				camera.Position -= camera.Attitude.Up;
			}

			Point center = new Point(Width / 2, Height / 2);
			PointF diff = new PointF((Mouse.X - center.X) / 100.0f, (Mouse.Y - center.Y) / 100.0f);

			if (Mouse[MouseButton.Right])
			{
				camera.Position = camera.Position 
								+ camera.Attitude.Side * diff.X 
								+ camera.Attitude.Up * diff.Y;
			}

			if (Mouse[MouseButton.Left])
			{
				Matrix4 pitch = Matrix4.Rotate(camera.Attitude.Side,
											   -(float)Math.PI * diff.Y / Height);

				Matrix4 yaw = Matrix4.Rotate(camera.Attitude.Up,
											   -(float)Math.PI * diff.X / Width);

				Matrix4 rotate = yaw * pitch;

				camera.Attitude = new Attitude(Vector3.TransformVector(camera.Attitude.Direction, rotate),
											   Vector3.TransformVector(camera.Attitude.Up, rotate));
			}
		}

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

		public override void OnRenderFrame(RenderFrameEventArgs e)
		{
			GL.ClearColor(ClearColor);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Enable(EnableCap.DepthTest);
			GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1); 

			UpdateProjection();

			#region Modelview
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			Glu.LookAt(camera.Position,
					   camera.Position + camera.Attitude.Direction,
					   camera.Attitude.Up);
			#endregion

			objects.Objects["Camera Position"] = camera.Position.ToString(2);
			objects.Objects["Camera Direction"] = camera.Attitude.Direction.ToString(2);
			objects.Objects["Camera Up"] = camera.Attitude.Up.ToString(2);
			objects.Objects["Camera Side"] = camera.Attitude.Side.ToString(2);

			Renderables.Render(GetRenderInfo());

			SwapBuffers();
		}
	}
}
