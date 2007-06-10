using System;
using Tesseract.Backends;
using Tesseract.Geometry;

namespace Tesseract.Graphics
{
	public class Solid: Pattern
	{
		public Solid(Color c, PatternType t)
		{
			this.color = c;
			this.Type = t;
		}
		
		public Solid(Color c): this(c, PatternType.Fill) { }
		public Solid(): this(Colors.White) { }
		
		Color color;
		public Color Color
		{
			get { return color; }
			set { color = value; }
		}

        public override void Apply(IGraphics g, double W, double H)
        {
            g.Solid(color.A, color.R, color.G, color.B);
        }
		
		public override string ToString()
   		{
   			return string.Format("Solid[{0}]", color);
   		}
	}
}
