using System;
using Tesseract.Controls;

namespace Tesseract.Geometry
{
	public class Padding
	{
		public Padding(Control C, Measurement L, Measurement T, Measurement R, Measurement B)
		{
			this.control = C;
			this.l = L;
			this.t = T;
			this.r = R;
			this.b = B;
		}
		
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
	}
}
