using System;

namespace Tesseract.Graphics
{
	public class GradientStop
	{
		public GradientStop(double s, Color c)
		{
			this.stop = s;
			this.color = c;
		}
		
		double stop;
		public double Stop
		{
			get { return stop; }
			set { stop = value; }
		}
		
		Color color;
		public Color Color
		{
			get { return color; }
			set { color = value; }
		}
	}
}
