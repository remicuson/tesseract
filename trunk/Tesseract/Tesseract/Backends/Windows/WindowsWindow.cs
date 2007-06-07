using System;
using Tesseract.Events;

namespace Tesseract.Backends
{
	public class WindowsWindow: System.Windows.Forms.Form, IWindow
	{
		public new event EventHandler<MouseEventArgs> MouseMove;
		public event EventHandler<MouseEventArgs> MousePress;
		public event EventHandler<MouseEventArgs> MouseRelease;
		public event EventHandler<RenderEventArgs> Render;
		public new event EventHandler Resize;
		
		public WindowsWindow()
		{
			this.DoubleBuffered = true;
		}
		
		public string Title
		{
			get { return base.Text; }
			set { base.Text = value; }
		}
		
		public double L
		{
			get { return base.Left; }
			set { base.Left = (int)value; }
		}
		
		public double T
		{
			get { return base.Top; }
			set { base.Top = (int)value; }
		}
		
		public double W
		{
			get { return base.ClientSize.Width; }
			set { base.ClientSize = new System.Drawing.Size((int)value, base.ClientSize.Height); }
		}
		
		public double H
		{
			get { return base.ClientSize.Height; }
			set { base.ClientSize = new System.Drawing.Size(base.ClientSize.Width, (int)value); }
		}
		
		public double DpiX
		{
			get
			{
				System.Drawing.Graphics g = base.CreateGraphics();
				double dpix = g.DpiX;
				g.Dispose();
				return dpix;
			}
		}
		
		public double DpiY
		{
			get
			{
				System.Drawing.Graphics g = base.CreateGraphics();
				double dpiy = g.DpiY;
				g.Dispose();
				return dpiy;
			}
		}
		
		public bool Framed
		{
			get { return base.FormBorderStyle == System.Windows.Forms.FormBorderStyle.Sizable; }
			set { base.FormBorderStyle = value ? System.Windows.Forms.FormBorderStyle.Sizable : System.Windows.Forms.FormBorderStyle.FixedSingle; }
		}
		
		public void ReRender()
		{
			base.Invalidate();
		}
		
		public void ReRender(double L, double T, double R, double B)
		{
			base.Invalidate(new System.Drawing.Rectangle((int)L, (int)T, (int)(R - L), (int)(B - T)));
		}
		
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			WindowsGraphics g = new WindowsGraphics(e.Graphics, W, H);
			
			if (this.Render != null)
				this.Render(this, new RenderEventArgs(g));
			
			g.Dispose();
		}

		protected override void OnResize(EventArgs e)
		{
			if (this.Resize != null)
				this.Resize(this, EventArgs.Empty);
		}

		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.MouseMove != null)
				this.MouseMove(this, new MouseEventArgs(MouseButton.None, e.X, e.Y));
		}

		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.MousePress != null)
				this.MousePress(this, new MouseEventArgs(GetMouseBtn(e.Button), e.X, e.Y));
		}
		
		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			if (this.MouseRelease != null)
				this.MouseRelease(this, new MouseEventArgs(GetMouseBtn(e.Button), e.X, e.Y));
		}

		MouseButton GetMouseBtn(System.Windows.Forms.MouseButtons b)
		{
			if (b == System.Windows.Forms.MouseButtons.Left)
				return MouseButton.Left;
			else if (b == System.Windows.Forms.MouseButtons.Middle)
				return MouseButton.Middle;
			else if (b == System.Windows.Forms.MouseButtons.Right)
				return MouseButton.Right;
			
			return MouseButton.None;
		}
	}
}
