using System;
using Tesseract.Backends;

namespace Tesseract.Geometry
{
	public class Rectangle: Path
	{
		public Rectangle(Measurement W, Measurement H)
		{
			base.W = W;
			base.H = H;
		}
	}
}
