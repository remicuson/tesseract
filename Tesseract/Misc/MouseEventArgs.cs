using System;
using Tesseract.Geometry;

namespace Tesseract.Events
{
	public enum MouseButton { None, Left, Right, Middle }
	
	public class MouseEventArgs: EventArgs
	{
		public MouseEventArgs(MouseButton B, Distance X, Distance Y)
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
		
		Distance x;
		public Distance X
		{
			get { return x; }
			set { x = value; }
		}
		
		Distance y;
		public Distance Y
		{
			get { return y; }
			set { y = value; }
		}
	}
}
