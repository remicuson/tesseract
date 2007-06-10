using System;
using System.Collections;
using System.Collections.Generic;
using Tesseract.Backends;
using Tesseract.Geometry;

namespace Tesseract.Graphics
{
	public enum LinearGradientOrientation { Horizontal, Vertical, Diagonal }
	public class LinearGradient: Pattern
	{
		public LinearGradient(LinearGradientOrientation o)
		{
			this.orientation = o;
		}
		
		public LinearGradient(): this(LinearGradientOrientation.Vertical) { }
		
		public LinearGradient(LinearGradientOrientation o, Color c1, Color c2): this(o)
		{
			this.Add(new GradientStop(0, c1));
			this.Add(new GradientStop(1, c2));
		}
		
		public LinearGradient(Color c1, Color c2): this(LinearGradientOrientation.Vertical, c1, c2) { }
		
		LinearGradientOrientation orientation;
		public LinearGradientOrientation Orientation
		{
			get { return orientation; }
			set { orientation = value; }
		}
		
		List<GradientStop> stops = new List<GradientStop>();
		public List<GradientStop> Stops
		{
			get { return stops; }
			set { stops = value; }
		}

		public void Add(GradientStop stop)
		{
			stops.Add(stop);
		}
	}
}
