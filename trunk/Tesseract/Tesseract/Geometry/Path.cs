using System;
using Tesseract.Backends;
using Tesseract.Controls;
using Tesseract.Geometry;

namespace Tesseract
{
	public class Path
	{
		public Path()
		{
		}
		
		Control control;
		public Control Control
		{
			get { return control; }
			set { control = value; }
		}
		
		Measurement w;
		public Measurement W
		{
			get { return w; }
			set
			{
				w = value;
				
				if (w != null)
					w.Orientation = MeasurementOrientation.Horizontal;
				
				if (control != null)
					control.OnResize();
			}
		}
		
		Measurement h;
		public Measurement H
		{
			get { return h; }
			set
			{
				h = value;
				
				if (h != null)
					h.Orientation = MeasurementOrientation.Vertical;
				
				if (control != null)
					control.OnResize();
			}
		}
		
		public virtual bool Contains(Measurement X, Measurement Y)
		{
			return (X >= 0) & (Y >= 0) & (X <= W) & (Y <= H);
		}
	}
}
