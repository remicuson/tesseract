using System;
using Tesseract.Backends;
using Tesseract.Geometry;
using Tesseract.Graphics;

namespace Tesseract.Controls
{
	public class Button: MouseStateControl
	{
		public Button(): base()
		{
			this.Path = new RoundedRectangle(80, 25, 5);
			this.Padding = new Padding(this, 5, 5, 5, 5);
			this.AutoSize = true;
			
			Core.themer.InitButton(this);
		}
		
		public override bool StealChildMouse(Control child, Measurement X, Measurement Y)
		{
			return true;
		}
		
		public override void RenderControl(IGraphics g)
		{
			Core.themer.RenderButton(this, g);
		}
	}
}
