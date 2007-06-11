using System;
using System.Collections;
using System.Collections.Generic;
using Tesseract.Backends;
using Tesseract.Geometry;

namespace Tesseract.Graphics
{
	public class RadialGradient: Pattern
	{
		public RadialGradient(double X1, double Y1, double R1, double X2, double Y2, double R2)
		{
			this.x1 = X1;
			this.y1 = Y1;
			this.r1 = R1;
			this.x2 = X2;
			this.y2 = Y2;
			this.r2 = R2;
		}
		
		public RadialGradient(double R1, double R2): this(0, 0, R1, 0, 0, R2) { }
		public RadialGradient(): this(0.2, 0.5) { }
		
		double x1;
		public double X1
		{
			get { return x1; }
			set { x1 = value; }
		}
		
		double y1;
		public double Y1
		{
			get { return y1; }
			set { y1 = value; }
		}
		
		double r1;
		public double R1
		{
			get { return r1; }
			set { r1 = value; }
		}
		
		double x2;
		public double X2
		{
			get { return x2; }
			set { x2 = value; }
		}
		
		double y2;
		public double Y2
		{
			get { return y2; }
			set { y2 = value; }
		}
		
		double r2;
		public double R2
		{
			get { return r2; }
			set { r2 = value; }
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

            g.RadialGradient(x1, y1, r1, x2, y2, r2, S, A, R, G, B);
        }
	}
}
