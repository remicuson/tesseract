using System;
using Tesseract.Backends;
using Tesseract.Controls;

namespace Tesseract.Geometry
{
	public class Location
	{
		public Location(Control C, Measurement L, Measurement T, Measurement R, Measurement B)
		{
			this.control = C;
			this.l = L;
			this.t = T;
			this.r = R;
			this.b = B;
		}
		
		public Location(): this(Tesseract.TIM.TIM.currentControl, null, null, null, null) { }
		
		Control control;
		public Control Control
		{
			get { return control; }
			set { control = value; }
		}
		
		Measurement l;
		public Measurement L
		{
			get { return l; }
			set
			{
				l = value;
				
				if (l != null)
					l.Orientation = MeasurementOrientation.Horizontal;
			}
		}
		
		Measurement t;
		public Measurement T
		{
			get { return t; }
			set
			{
				t = value;
				
				if (t != null)
					t.Orientation = MeasurementOrientation.Vertical;
			}
		}
		
		Measurement r;
		public Measurement R
		{
			get { return r; }
			set
			{
				r = value;
				
				if (r != null)
					r.Orientation = MeasurementOrientation.Horizontal;
			}
		}
		
		Measurement b;
		public Measurement B
		{
			get { return b; }
			set
			{
				b = value;
				
				if (b != null)
					b.Orientation = MeasurementOrientation.Vertical;
			}
		}
		
		public Measurement RealL
		{
			get
			{
				if (control.Margin.L == null && control.Margin.R == null)
					return (control.Parent.Path.W - control.Path.W) / 2;
				
				double x = 0;
				
				if (l != null && r != null)
					x = l;
				else if (l != null)
					x = l;
				else if (r != null)
					x = control.Parent.Path.W - control.Path.W - Math.Max(r, control.Margin.R);
					
				if (x < control.Margin.L)
					x = control.Margin.L;
				
				return x;
			}
		}
		
		public Measurement RealT
		{
			get
			{
				if (control.Margin.T == null && control.Margin.B == null)
					return (control.Parent.Path.H - control.Path.H) / 2;
					
				double y = 0;
			
				if (t != null && b != null)
					y = t;
				else if (t != null)
					y = t;
				else if (b != null)
					y = control.Parent.Path.H - control.Path.H - Math.Max(b, control.Margin.B);
					
				if (y < control.Margin.T)
					y = control.Margin.T;
				
				return y;
			}
		}
		
		public void HandleRel()
		{
			if (l != null && r != null)
				control.Path.W = control.Parent.Path.W - Math.Max(r, control.Margin.R) - Math.Max(l, control.Margin.L);
			
			if (t != null && b != null)
				control.Path.H = control.Parent.Path.H - Math.Max(b, control.Margin.B) - Math.Max(t, control.Margin.T);
		}
		
		public void Offset(Measurement X, Measurement Y)
		{
			if (l != null)
				l += X;
			else if (r != null)
				r -= X;
			
			if (t != null)
				t += Y;
			else if (b != null)
				b -= Y;
		}
		
		public Location Clone()
		{
			return new Location(control, l, t, r, b);
		}
	}
}
