using System;
using Tesseract.Backends;

namespace Tesseract.Geometry
{
	public class RoundedRectangle: Path
	{
		public RoundedRectangle(Measurement W, Measurement H, Measurement RTL, Measurement RTR, Measurement RBL, Measurement RBR)
		{
			base.W = W;
			base.H = H;
			this.rtl = RTL;
			this.rtr = RTR;
			this.rbl = RBL;
			this.rbr = RBR;
		}
		
		public RoundedRectangle(Measurement W, Measurement H, Measurement RT, Measurement RB): this(W, H, RT, RT, RB, RB) { }
		public RoundedRectangle(Measurement W, Measurement H, Measurement R): this(W, H, R, R) { }
		
		Measurement rtl;
		public Measurement RTL
		{
			get { return rtl; }
			set { rtl = value; }
		}
		
		Measurement rtr;
		public Measurement RTR
		{
			get { return rtr; }
			set { rtr = value; }
		}
		
		Measurement rbl;
		public Measurement RBL
		{
			get { return rbl; }
			set { rbl = value; }
		}
		
		Measurement rbr;
		public Measurement RBR
		{
			get { return rbr; }
			set { rbr = value; }
		}
		
		public Measurement RT
		{
			set
			{
				RTL = value;
				RTR = value;
			}
		}
		
		public Measurement RB
		{
			set
			{
				RBL = value;
				RBR = value;
			}
		}
		
		public Measurement R
		{
			set
			{
				RT = value;
				RB = value;
			}
		}
	}
}
