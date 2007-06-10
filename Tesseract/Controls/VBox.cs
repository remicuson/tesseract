using System;
using Tesseract.Backends;

namespace Tesseract.Controls
{
	public class VBox: Control
	{
		public VBox()
		{
			Core.themer.InitVBox(this);
		}
		
		public override void RenderControl(IGraphics g)
		{
			Core.themer.RenderVBox(this, g);
		}
	}
}
