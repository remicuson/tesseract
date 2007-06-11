using System;
using System.Runtime.InteropServices; 
using Tesseract.Events;
using Tesseract.Controls;

namespace Tesseract.Backends.Windows
{
	public class WindowsWindow: System.Windows.Forms.Form, IWindow
	{
		public new event EventHandler<MouseEventArgs> MouseMove;
		public event EventHandler<MouseEventArgs> MousePress;
		public event EventHandler<MouseEventArgs> MouseRelease;
		public event EventHandler<RenderEventArgs> Render;
		public new event EventHandler Resize;

        System.Drawing.Bitmap bitmap;

        #region For Client Area Drag

        const int WM_NCLBUTTONDOWN = 0xA1;
        const int WM_NCRBUTTONDOWN = 0xA4;
        const int HTCAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        static extern bool ReleaseCapture(); 

        #endregion

        public WindowsWindow()
		{
			this.DoubleBuffered = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		}

        /*protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00080000; //WS_EX_LAYERED
                return cp;
            }
        }*/

        Window window;
        public Window Window
        {
            get { return window; }
            set { window = value; }
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
			set
            {
                base.FormBorderStyle = value ? System.Windows.Forms.FormBorderStyle.Sizable : System.Windows.Forms.FormBorderStyle.None;
            
                //if (!value)
                //    Win32.SetWindowLong(this.Handle, Win32.GWL_EXSTYLE, Win32.WS_EX_LAYERED);
            }
		}
		
		public void ReRender()
		{
            //if (!Framed)
            //    PerformRender();

			base.Invalidate();
		}
		
		public void ReRender(double L, double T, double R, double B)
		{
            //if (!Framed)
            //    PerformRender();

			base.Invalidate(new System.Drawing.Rectangle((int)L, (int)T, (int)(R - L), (int)(B - T)));
		}

        protected override void Dispose(bool disposing)
        {
            if (bitmap != null)
                bitmap.Dispose();

            base.Dispose(disposing);
        }

        void UpdatePPMask(System.Drawing.Bitmap bitmap)
        {
            IntPtr screenDc = Win32.GetDC(IntPtr.Zero);
            IntPtr memDc = Win32.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
                hBitmap = bitmap.GetHbitmap(System.Drawing.Color.FromArgb(0));
                oldBitmap = Win32.SelectObject(memDc, hBitmap);

                Win32.Size size = new Win32.Size(bitmap.Width, bitmap.Height);
                Win32.Point pointSource = new Win32.Point(0, 0);
                Win32.Point topPos = new Win32.Point(Left, Top);
                Win32.BLENDFUNCTION blend = new Win32.BLENDFUNCTION();
                blend.BlendOp = Win32.AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = 255;
                blend.AlphaFormat = Win32.AC_SRC_ALPHA;

                Win32.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, Win32.ULW_ALPHA);
            }
            finally
            {
                Win32.ReleaseDC(IntPtr.Zero, screenDc);

                if (hBitmap != IntPtr.Zero)
                {
                    Win32.SelectObject(memDc, oldBitmap);
                    Win32.DeleteObject(hBitmap);
                }

                Win32.DeleteDC(memDc);
            }
        }
		
		protected void PerformRender()
		{
            if (bitmap != null)
                bitmap.Dispose();

            bitmap = new System.Drawing.Bitmap((int)W, (int)H, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Drawing.Graphics wg = System.Drawing.Graphics.FromImage(bitmap);
            wg.Clear(System.Drawing.Color.Transparent);

			WindowsGraphics g = new WindowsGraphics(wg, W, H);
			
			if (this.Render != null)
				this.Render(this, new RenderEventArgs(g));
			
			g.Dispose();
            wg.Dispose();

            UpdatePPMask(bitmap);
		}

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            //if (Framed)
            //{
                WindowsGraphics g = new WindowsGraphics(e.Graphics, W, H);

                if (this.Render != null)
                    this.Render(this, new RenderEventArgs(g));

                g.Dispose();
            //}
            //else
            //    base.OnPaint(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            PerformRender();
        }

		protected override void OnResize(EventArgs e)
		{
			if (this.Resize != null)
				this.Resize(this, EventArgs.Empty);

            this.Invalidate();
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

            if (window.mouseOverControl.WindowDrag)
            {
                ReleaseCapture();

                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                else
                    SendMessage(Handle, WM_NCRBUTTONDOWN, HTCAPTION, 0);
            }
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
