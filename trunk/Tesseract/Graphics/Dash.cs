using System;
using Tesseract.Backends;
using Tesseract.Geometry;

namespace Tesseract.Graphics
{
	public class Dash: Solid
	{
		public Dash(Color C, Distance Size): base(C)
		{
			this.size = Size;
		}
		
		public Dash(Color C): this(C, 8) { }
		public Dash(): this(Colors.White) { }
		
		Distance size;
		public Distance Size
		{
			get { return size; }
			set { size = value; }
		}

        public override void Apply(IGraphics g, double W, double H)
        {
            base.Apply(g, W, H);
            g.Dash(size);
        }
	}
}
