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
	public class ObjectDisplay : IRenderable
	{
		private TextPrinter textPrinter;
		private string text;

		public ObjectDisplay()
		{
			this.textPrinter = new TextPrinter(TextQuality.High);
			this.Objects = new Dictionary<string, object>();
			this.text = string.Empty;
		}

		public Dictionary<string, object> Objects { get; private set; }

		public Color Color { get; set; }

		public PointF Location { get; set; }

		public bool Initialized
		{
			get { return true; }
		}

		public void Initialize() { }

		public bool Update(RenderInfo info)
		{
			StringBuilder textBuilder = new StringBuilder();

			foreach (KeyValuePair<string, object> item in Objects)
			{
				textBuilder.AppendFormat("{0}: {1}{2}", item.Key, item.Value, Environment.NewLine);
			}

			text = textBuilder.ToString();

			return Objects.Count > 0;
		}

		public void Render(RenderInfo info)
		{
			GL.Disable(EnableCap.Texture2D);
			GL.UseProgram(0);

			textPrinter.Begin();

			Font font = Control.DefaultFont;
			SizeF size = new SizeF(500, Objects.Count * (2 * font.SizeInPoints));

			textPrinter.Print(text, font, Color, new RectangleF(Location, size));

			textPrinter.End();
		}
	}
}
