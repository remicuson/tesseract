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

        public Rectangle() : this(10, 10) { }

        public override void Apply(IGraphics g)
        {
            g.ClearPath();
            g.Rectangle(0, 0, W - 1, H - 1);
        }
	}
}
