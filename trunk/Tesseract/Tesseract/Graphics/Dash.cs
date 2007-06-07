using System;
using Tesseract.Backends;
using Tesseract.Geometry;

namespace Tesseract.Graphics
{
	public class Dash: Solid
	{
		public Dash(Color C, Measurement Size): base(C)
		{
			this.size = Size;
		}
		
		public Dash(Color C): this(C, 8) { }
		public Dash(): this(Colors.White) { }
		
		Measurement size;
		public Measurement Size
		{
			get { return size; }
			set { size = value; }
		}
	}
}
