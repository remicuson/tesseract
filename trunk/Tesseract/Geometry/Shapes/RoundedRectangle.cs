using System;
using Tesseract.Backends;

namespace Tesseract.Geometry
{
    /// <summary>
    /// A path representing a rectangle with rounded corners
    /// </summary>
	public class RoundedRectangle: Path
	{
		public RoundedRectangle(Distance W, Distance H, Distance RTL, Distance RTR, Distance RBL, Distance RBR)
		{
			base.W = W;
			base.H = H;
			this.rtl = RTL;
			this.rtr = RTR;
			this.rbl = RBL;
			this.rbr = RBR;
		}
		
		public RoundedRectangle(Distance W, Distance H, Distance RT, Distance RB): this(W, H, RT, RT, RB, RB) { }
		public RoundedRectangle(Distance W, Distance H, Distance R): this(W, H, R, R) { }
        public RoundedRectangle() : this(10, 10, 2) { }

    	Distance rtl;
		/// <summary>
		/// The distance from the top left corner at which rounding begins
		/// </summary>
        public Distance RTL
		{
			get { return rtl; }
			set { rtl = value; }
		}
		
		Distance rtr;
        /// <summary>
        /// The distance from the top right corner at which rounding begins
        /// </summary>
		public Distance RTR
		{
			get { return rtr; }
			set { rtr = value; }
		}
		
		Distance rbl;
        /// <summary>
        /// The distance from the bottom left corner at which rounding begins
        /// </summary>
		public Distance RBL
		{
			get { return rbl; }
			set { rbl = value; }
		}
		
		Distance rbr;
        /// <summary>
        /// The distance from the bottom right corner at which rounding begins
        /// </summary>
		public Distance RBR
		{
			get { return rbr; }
			set { rbr = value; }
		}

        /// <summary>
        /// Sets both RTL & RTR
        /// </summary>
		public Distance RT
		{
			set
			{
				RTL = value;
				RTR = value;
			}
		}
		
        /// <summary>
        /// Sets both RBL & RBR
        /// </summary>
		public Distance RB
		{
			set
			{
				RBL = value;
				RBR = value;
			}
		}
		
        /// <summary>
        /// Sets both RT & RB (therefore setting RTL, RTR, RBL & RTR)
        /// </summary>
		public Distance R
		{
			set
			{
				RT = value;
				RB = value;
			}
		}

        public override void Apply(IGraphics g)
        {
            g.ClearPath();
            g.RoundedRectangle(0, 0, W - 1, H - 1, RTL, RTR, RBL, RBR);
        }

        public override Path Clone()
        {
            return new RoundedRectangle(W.Clone(), H.Clone(), RTL.Clone(), RTR.Clone(), RBL.Clone(), RBR.Clone());
        }

        public override Path ClonePixels()
        {
            return new RoundedRectangle(W.Pixels, H.Pixels, RTL.Pixels, RTR.Pixels, RBL.Pixels, RBR.Pixels);
        }
	}
}
