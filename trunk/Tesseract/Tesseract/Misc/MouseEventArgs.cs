using System;
using Tesseract.Geometry;

namespace Tesseract.Events
{
	public enum MouseButton { None, Left, Right, Middle }
	
	public class MouseEventArgs: EventArgs
	{
		public MouseEventArgs(MouseButton B, Measurement X, Measurement Y)
		{
			this.btn = B;
			this.x = X;
			this.y = Y;
		}
		
		MouseButton btn;
		public MouseButton Button
		{
			get { return btn; }
			set { btn = value; }
		}
		
		Measurement x;
		public Measurement X
		{
			get { return x; }
			set { x = value; }
		}
		
		Measurement y;
		public Measurement Y
		{
			get { return y; }
			set { y = value; }
		}
	}
}
