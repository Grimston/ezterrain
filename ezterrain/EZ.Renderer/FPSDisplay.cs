using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Core;
using OpenTK.Graphics;
using System.Drawing;
using System.Windows.Forms;

namespace EZ.Renderer
{
	public class FPSDisplay : IRenderable
	{
		private TextPrinter textPrinter;
		private double accumTime;
		private LinkedList<double> timeBuffer;

		public FPSDisplay()
		{
			textPrinter = new TextPrinter(TextQuality.High);
			this.Color = Color.Black;
			AccumCount = 10;
			accumTime = 0;
			timeBuffer = new LinkedList<double>();
		}

		public Color Color { get; set; }

		public uint AccumCount { get; set; }

		public PointF Location { get; set; }

		#region IRenderable Members

		public bool Initialized
		{
			get { return true; }
		}

		public void Initialize() { }

		public bool Update(RenderInfo info)
		{
			timeBuffer.AddLast(info.FrameStamp.DeltaTime);
			accumTime += info.FrameStamp.DeltaTime;

			while (timeBuffer.Count > AccumCount)
			{
				accumTime -= timeBuffer.First.Value;
				timeBuffer.RemoveFirst();
			}

			return true;
		}

		public void Render(RenderInfo info)
		{
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.Disable(EnableCap.Texture2D);
			GL.UseProgram(0);
			GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);

			textPrinter.Begin();

			double spf = accumTime / timeBuffer.Count;

			textPrinter.Print(string.Format("SPF: {0:F3}{1}FPS: {2}", 
											spf, 
											Environment.NewLine, 
											Math.Round(1 / spf)),
							  Control.DefaultFont,
							  Color,
							  new RectangleF(Location, new SizeF(200, 40)));

			textPrinter.End();
		}

		#endregion
	}
}
