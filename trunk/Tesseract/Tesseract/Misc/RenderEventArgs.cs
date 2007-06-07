using System;
using Tesseract.Backends;

namespace Tesseract.Events
{
	public class RenderEventArgs: EventArgs
	{
		public RenderEventArgs(IGraphics g)
		{
			this.graphics = g;
		}
		
		IGraphics graphics;
		public IGraphics Graphics
		{
			get { return graphics; }
		}
	}
}
