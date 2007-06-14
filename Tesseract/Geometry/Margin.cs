using System;
using Tesseract.Controls;

namespace Tesseract.Geometry
{
	public class Margin
	{
		public Margin(Control C, Distance L, Distance T, Distance R, Distance B)
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
		
		Distance l;
		public Distance L
		{
			get { return l; }
			set
			{
				l = value;
				
				if (l != null)
					l.Orientation = DistanceOrientation.Horizontal;
			}
		}
		
		Distance t;
		public Distance T
		{
			get { return t; }
			set
			{
				t = value;
				
				if (t != null)
					t.Orientation = DistanceOrientation.Vertical;
			}
		}
		
		Distance r;
		public Distance R
		{
			get { return r; }
			set
			{
				r = value;
				
				if (r != null)
					r.Orientation = DistanceOrientation.Horizontal;
			}
		}
		
		Distance b;
		public Distance B
		{
			get { return b; }
			set
			{
				b = value;
				
				if (b != null)
					b.Orientation = DistanceOrientation.Vertical;
			}
		}
	}
}
