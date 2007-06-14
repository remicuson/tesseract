using System;
using Tesseract.Backends;

namespace Tesseract.Geometry
{
    /// <summary>
    /// A path representing a simple rectangle
    /// </summary>
	public class Rectangle: Path
	{
		public Rectangle(Distance W, Distance H)
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

        public override Path Clone()
        {
            return new Rectangle(
                W != null ? W.Clone() : null, 
                H != null ? H.Clone() : null);
        }

        public override Path ClonePixels()
        {
            return new Rectangle(
                W != null ? W.Pixels : 0,
                H != null ? H.Pixels : 0);
        }

        public override string ToString()
        {
            return string.Format("Rectangle[{0}x{1}]", W.Pixels, H.Pixels);
        }
	}
}
