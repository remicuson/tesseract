using System;
using Tesseract.Backends;
using Tesseract.Controls;
using Tesseract.Geometry;

namespace Tesseract
{
    /// <summary>
    /// The base of all path objects
    /// </summary>
	public class Path
	{
		public Path()
		{
		}
		
		Control control;
        /// <summary>
        /// The Control associated with this path
        /// </summary>
		public Control Control
		{
			get { return control; }
			set
            { 
                control = value;

                if (w != null)
                    w.Control = value;
                if (h != null)
                    h.Control = value;
            }
		}
		
		Distance w;
        /// <summary>
        /// The width of the path
        /// </summary>
		public Distance W
		{
			get { return w; }
			set
			{
				w = value;

                if (w != null)
                {
                    w.Orientation = DistanceOrientation.Horizontal;
                    w.Control = control;
                }
				
				if (control != null)
					control.OnResize();
			}
		}
		
		Distance h;
        /// <summary>
        /// The height of the path
        /// </summary>
		public Distance H
		{
			get { return h; }
			set
			{
				h = value;

                if (h != null)
                {
                    h.Orientation = DistanceOrientation.Vertical;
                    h.Control = control;
                }
				
				if (control != null)
					control.OnResize();
			}
		}

        /// <summary>
        /// Applies the path to a backend graphics interface
        /// </summary>
        /// <param name="g">The backend graphics interface to use</param>
        public virtual void Apply(IGraphics g)
        {

        }
		
        /// <summary>
        /// Detects if a point is inside this path
        /// </summary>
        /// <param name="X">The X co-ordinate of the point</param>
        /// <param name="Y">The Y co-ordinate of the point</param>
        /// <returns>True if the path contains the point, otherwise false</returns>
		public virtual bool Contains(Distance X, Distance Y)
		{
			return (X >= 0) & (Y >= 0) & (X <= W) & (Y <= H);
		}

        /// <summary>
        /// Creates an exact copy of this path
        /// </summary>
        /// <returns>The copy of this path</returns>
        public virtual Path Clone()
        {
            return new Path();
        }

        /// <summary>
        /// Creates a copy of this path, but with all units converted to pixels
        /// </summary>
        /// <returns>The copy of this path</returns>
        public virtual Path ClonePixels()
        {
            return new Path();
        }
	}
}
