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

        public override void Apply(IGraphics g, double W, double H)
        {
            g.StrokeSize = this.StrokeSize;

            double[] S = new double[stops.Count];
            double[] A = new double[stops.Count];
            double[] R = new double[stops.Count];
            double[] G = new double[stops.Count];
            double[] B = new double[stops.Count];

            for (int i = 0; i < stops.Count; i++)
            {
                S[i] = stops[i].Stop;
                A[i] = stops[i].Color.A;
                R[i] = stops[i].Color.R;
                G[i] = stops[i].Color.G;
                B[i] = stops[i].Color.B;
            }

            double x = (orientation == LinearGradientOrientation.Vertical) ? 0 : W;
            double y = (orientation == LinearGradientOrientation.Horizontal) ? 0 : H;

            g.LinearGradient(0, 0, x, y, S, A, R, G, B);
        }
	}
}
