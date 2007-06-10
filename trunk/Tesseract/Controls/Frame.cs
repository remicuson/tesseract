using System;
using Tesseract.Backends;
using Tesseract.Graphics;

namespace Tesseract.Controls
{
	public class Frame: MouseStateControl
	{
		public Frame(): base()
		{
			Core.themer.InitFrame(this);
		}
		
		public override void RenderControl(IGraphics g)
		{
			Core.themer.RenderFrame(this, g);
		}
	}
}
