using System;
using Tesseract.Backends;

namespace Tesseract.Controls
{
	public class HBox: Control
	{
		public HBox()
		{
			Core.themer.InitHBox(this);
		}
		
		public override void RenderControl(IGraphics g)
		{
			Core.themer.RenderHBox(this, g);
		}
	}
}
