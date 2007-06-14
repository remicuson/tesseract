using System;
using Tesseract.Backends;
using Tesseract.Geometry;
using Tesseract.Graphics;

namespace Tesseract.Controls
{
	public class Button: Control
	{
		public Button(): base()
		{
			this.Path = new RoundedRectangle(80, 25, 5);
			this.Padding = new Padding(this, 5, 5, 5, 5);
			this.AutoSize = true;
		}
		
		public override bool StealChildMouse(Control child, Distance X, Distance Y)
		{
			return true;
		}
	}
}
