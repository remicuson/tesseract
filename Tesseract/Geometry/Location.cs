using System;
using Tesseract.Backends;
using Tesseract.Controls;

namespace Tesseract.Geometry
{
    /// <summary>
    /// Represents the location of a control
    /// </summary>
	public class Location
	{
		public Location(Control C, Distance L, Distance T, Distance R, Distance B)
		{
			this.control = C;
			this.l = L;
			this.t = T;
			this.r = R;
			this.b = B;
		}
		
		public Location(): this(Tesseract.TIM.TIM.currentControl, null, null, null, null) { }
		
		Control control;
        /// <summary>
        /// The control associated with this location
        /// </summary>
		public Control Control
		{
			get { return control; }
			set
            {
                control = value;

                if (l != null)
                    l.Control = value;
                if (t != null)
                    t.Control = value;
                if (r != null)
                    r.Control = value;
                if (b != null)
                    b.Control = value;
            }
		}
		
		Distance l;
		public Distance L
		{
			get { return l; }
			set
			{
				l = value;

                if (l != null)
                {
                    l.Orientation = DistanceOrientation.Horizontal;
                    l.Control = control;
                }
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
                {
                    t.Orientation = DistanceOrientation.Vertical;
                    t.Control = control;
                }
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
                {
                    r.Orientation = DistanceOrientation.Horizontal;
                    r.Control = control;
                }
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
                {
                    b.Orientation = DistanceOrientation.Vertical;
                    b.Control = control;
                }
			}
		}
		
        /// <summary>
        /// The distance from the left of the controls parent at which to render the control
        /// </summary>
		public Distance RealL
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

		/// <summary>
		/// The distance from the top of the controls parent at which to render the control
		/// </summary>
		public Distance RealT
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
		
        /// <summary>
        /// Resizes the controls path if the location dictates that the path is relative to the controls parents
        /// </summary>
		public void HandleRel()
		{
			if (l != null && r != null)
				control.Path.W = control.Parent.Path.W - Math.Max(r, control.Margin.R) - Math.Max(l, control.Margin.L);
			
			if (t != null && b != null)
				control.Path.H = control.Parent.Path.H - Math.Max(b, control.Margin.B) - Math.Max(t, control.Margin.T);
		}
		
        /// <summary>
        /// Offsets the location by a given amount
        /// </summary>
        /// <param name="X">The distance to offset the location horizontally</param>
        /// <param name="Y">The distance to offset the location vertically</param>
		public void Offset(Distance X, Distance Y)
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
		
        /// <summary>
        /// Creates an exact copy of this location
        /// </summary>
        /// <returns></returns>
		public Location Clone()
		{
			return new Location(control, l, t, r, b);
		}

        public override string ToString()
        {
            return string.Format("Location[{0},{1}  {2},{3}]", 
                (l != null) ? l.Pixels.ToString() : "null",
                (t != null) ? t.Pixels.ToString(): "null",
                (r != null) ? r.Pixels.ToString() : "null",
                (b != null) ? b.Pixels.ToString() : "null");
        }
	}
}
